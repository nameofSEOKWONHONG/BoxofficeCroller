using SharpRepository.EfRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BoxOfficeCroller
{
    public class BoxofficeRepository
    {
        public void SetAddBoxOffice(BOXOFFICE_MASTER boxOfficeMaster)
        {
            try {
                var tranoptions = new TransactionOptions();
                tranoptions.IsolationLevel = IsolationLevel.ReadCommitted;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, tranoptions))
                {
                    using (var master = new EfRepository<BOXOFFICE_MASTER>(new BoxofficeDbContext()))
                    {
                        master.Add(boxOfficeMaster);

                        using (var detail = new EfRepository<BOXOFFICE_DETAIL>(new BoxofficeDbContext()))
                        {
                            foreach (var boxOfficeDetail in boxOfficeMaster.BoxOfficeDetails)
                            {
                                boxOfficeDetail.MASTER_IDX = boxOfficeMaster.IDX;
                                detail.Add(boxOfficeDetail);
                            }
                        }
                    }

                    scope.Complete();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        public BOXOFFICE_MASTER GetBoxOfficeMaster(string regDt)
        {
            using(var context = new BoxofficeDbContext())
            {
                return context.TA_NEO_BOXOFFICE_MASTER.Where(m => m.REG_DT == regDt).FirstOrDefault();
            }
        }
    }
}
