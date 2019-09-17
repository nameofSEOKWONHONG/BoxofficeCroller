using CefSharp;

namespace BoxOfficeCroller
{
    internal class RenderProcessMessageHandler : IRenderProcessMessageHandler
    {
        public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            //const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";

            //frame.ExecuteJavaScriptAsync(script);
        }

        public void OnContextReleased(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            //throw new System.NotImplementedException();
        }

        public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {
            //throw new System.NotImplementedException();
        }

        public void OnUncaughtException(IWebBrowser browserControl, IBrowser browser, IFrame frame, JavascriptException exception)
        {
            //throw new System.NotImplementedException();
        }
    }
}