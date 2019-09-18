using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BoxOfficeCroller
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// CefSharp browser variable defined
        /// </summary>
        private readonly ChromiumWebBrowser _browser;
        /// <summary>
        /// sync date (yyyyMMdd)
        /// </summary>
        private string _syncDate = string.Empty;
        /// <summary>
        /// kobis boxoffice url
        /// </summary>
        private const string KOBIS_BOXOFFICE_URL = "http://www.kobis.or.kr/kobis/business/stat/boxs/findDailyBoxOfficeList.do";
        /// <summary>
        /// total tr element count
        /// </summary>
        private const int ELEMENT_CNT = 9;

        public Form1()
        {
            InitializeComponent();

            #region init browser url and ui, event hanlder
            _browser = new ChromiumWebBrowser(KOBIS_BOXOFFICE_URL)
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(_browser);

            _browser.IsBrowserInitializedChanged += _browser_IsBrowserInitializedChanged; ;
            _browser.LoadingStateChanged += OnLoadingStateChanged;
            _browser.ConsoleMessage += OnBrowserConsoleMessage;
            _browser.StatusMessage += OnBrowserStatusMessage;
            _browser.TitleChanged += OnBrowserTitleChanged;
            _browser.AddressChanged += OnBrowserAddressChanged;

            _browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
            #endregion

            this.FormClosing += Form1_FormClosing;

        }

        private void _browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var b = ((ChromiumWebBrowser)sender);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_browser != null)
            {
                _browser.Dispose();
                Cef.Shutdown();
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _browser.Load(KOBIS_BOXOFFICE_URL);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {

        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs e)
        {

        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {

        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {

        }

        private async void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                this.Invoke(new MethodInvoker(
                     delegate ()
                     {
                         this.label2.Text = $"브라우저 크롤링 시작 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                     })
                );
                
                // Page has finished loading, do whatever you want here

                // sleep 2sec on first load time 
                Thread.Sleep(5000);

                // element click event
                for(var i = 0; i < ELEMENT_CNT; i++)
                {
                    _browser.ExecuteScriptAsync("document.getElementById('btn_0').click();");
                    Thread.Sleep(1000);

                    string htmltext = await _browser.GetSourceAsync();
                    HtmlAgilityPack.HtmlDocument htmlDocFindClose = new HtmlAgilityPack.HtmlDocument();
                    htmlDocFindClose.LoadHtml(htmltext);
                    var btn0 = htmlDocFindClose.GetElementbyId("btn_0");
                    if(btn0.InnerText.Contains("닫기"))
                    {
                        break;
                    }
                }

                // get html
                string HTML = await _browser.GetSourceAsync();
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(HTML);

                // select table
                var table = htmlDoc.GetElementbyId("table_0");
                // select tr
                var trs = table.SelectNodes(".//tr");

                this.Invoke(new MethodInvoker(
                     delegate ()
                     {
                         this.label2.Text = $"HTML 파싱 시작 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                     })
                );

                //define collection for boxoffice model
                BOXOFFICE_MASTER boxOfficeMaster = new BOXOFFICE_MASTER();
                boxOfficeMaster.REG_DT = DateTime.Now.ToString("yyyy-MM-dd");
                boxOfficeMaster.TITLE = $"{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")} {GetDay(DateTime.Now.AddDays(-1))}";
                boxOfficeMaster.BoxOfficeDetails = new List<BOXOFFICE_DETAIL>();

                //tr -> td loop
                foreach (var tr in trs)
                {
                    //get td
                    var tds = tr.SelectNodes(".//td");

                    //first row exception
                    if (tds == null) continue;

                    // define boxoffice model
                    var boxOfficeDetail = new BOXOFFICE_DETAIL();

                    // get td data
                    boxOfficeDetail.RANK = int.Parse(tds[0].InnerText.Replace("\n", "").Replace("\t", ""));
                    //var t = tds..DocumentNode.SelectNodes("//div[@class='ico_minus']")
                    var rankflag = tds[1].SelectNodes(".//span[@class='ico_minus']");
                    if (rankflag == null) {
                        rankflag = tds[1].SelectNodes(".//span[@class='ico_rise']");

                        if(rankflag == null)
                        {
                            rankflag = tds[1].SelectNodes(".//span[@class='ico_fall']");

                            if(rankflag == null)
                            {
                                boxOfficeDetail.RANK_FLAG = "M"; //변동없음
                                boxOfficeDetail.RANK_INC = 0;
                            }
                            else
                            {
                                boxOfficeDetail.RANK_FLAG = "D"; //하락
                                boxOfficeDetail.RANK_INC = int.Parse(rankflag[0].InnerText.Replace("하락", ""));
                            }
                        }
                        else
                        {
                            boxOfficeDetail.RANK_FLAG = "U"; //상승
                            boxOfficeDetail.RANK_INC = int.Parse(rankflag[0].InnerText.Replace("상승", ""));
                        }
                    }
                    else
                    {
                        boxOfficeDetail.RANK_FLAG = "M"; //변동없음
                        boxOfficeDetail.RANK_INC = 0;
                    }
                    
                    boxOfficeDetail.M_NM = tds[1].SelectNodes(".//a")[0].InnerText.Replace("\n", "").Replace("\t", "");

                    boxOfficeDetail.OPEN_DT = tds[2].InnerText.Replace("\n", "").Replace("\t", "");
                    if(string.IsNullOrEmpty(boxOfficeDetail.OPEN_DT.Replace(" ", "")))
                    {
                        boxOfficeDetail.OPEN_DT = "9999-12-31";
                    }

                    boxOfficeDetail.SALES = double.Parse(tds[3].InnerText.Replace("\n", "").Replace("\t", ""));
                    boxOfficeDetail.SALES_RATE = float.Parse(tds[4].InnerText.Replace("\n", "").Replace("\t", "").Replace("%", ""));
                    var tempSalesInc = tds[5].InnerText.Replace("\n", "").Replace("\t", "");
                    boxOfficeDetail.SALES_INC = double.Parse(tempSalesInc.Remove(tempSalesInc.IndexOf('(')));
                    boxOfficeDetail.SALES_CUM = double.Parse(tds[6].InnerText.Replace("\n", "").Replace("\t", ""));
                    boxOfficeDetail.ADN = double.Parse(tds[7].InnerText.Replace("\n", "").Replace("\t", ""));
                    var tempAdnInc = tds[8].InnerText.Replace("\n", "").Replace("\t", "");
                    boxOfficeDetail.ADN_INC = double.Parse(tempAdnInc.Remove(tempAdnInc.IndexOf('(')));
                    boxOfficeDetail.ADN_CUM = double.Parse(tds[9].InnerText.Replace("\n", "").Replace("\t", ""));
                    boxOfficeDetail.SCREEN_CNT = double.Parse(tds[10].InnerText.Replace("\n", "").Replace("\t", ""));
                    boxOfficeDetail.PLAY_TIMES = double.Parse(tds[11].InnerText.Replace("\n", "").Replace("\t", ""));

                    // add element to collection
                    boxOfficeMaster.BoxOfficeDetails.Add(boxOfficeDetail);
                }

                this.Invoke(new MethodInvoker(
                     delegate ()
                     {
                         this.label2.Text = $"DB 입력중 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                     })
                );

                //// here is insert to db
                var dac = new BoxofficeRepository();
                var searchResultMaster = dac.GetBoxOfficeMaster(DateTime.Now.ToString("yyyy-MM-dd"));
                if (searchResultMaster == null)
                {
                    dac.SetAddBoxOffice(boxOfficeMaster);
                }

                this.Invoke(new MethodInvoker(
                     delegate ()
                     {
                         this.label2.Text = $"준비중 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                     })
                );

                this.Invoke(new MethodInvoker(
                     delegate ()
                     {
                         this.Close();
                     })
                );
            }
        }

        private string GetDay(DateTime dt)
        {
            string strDay = "";

            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    strDay = "(월)";
                    break;
                case DayOfWeek.Tuesday:
                    strDay = "(화)";
                    break;
                case DayOfWeek.Wednesday:
                    strDay = "(수)";
                    break;
                case DayOfWeek.Thursday:
                    strDay = "(목)";
                    break;
                case DayOfWeek.Friday:
                    strDay = "(금)";
                    break;
                case DayOfWeek.Saturday:
                    strDay = "(토)";
                    break;
                case DayOfWeek.Sunday:
                    strDay = "(일)";
                    break;
            }

            return strDay;
        }
    }
}
