using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Sales;
using Infrastructure.UnitOfWork;
using Model.Sales.Interfaces;
using NHibernate;

namespace Repository.Repositories
{
    public class CreditSaleDetailRepository : Repository<CreditSaleDetail>, ICreditSaleDetailRepository
    {
        public CreditSaleDetailRepository(IUnitOfWork uow)
            : base(uow)
        {
        }

        
        public long FindSumOfLineTotal(IList<string> hqlQuery)
        {
            long result=0;
            result += hqlQuery.Select(item => SessionFactory.GetCurrentSession().CreateQuery(item)).Select(qry => qry.UniqueResult<long>()).Sum();

            return result;
        }
    }
}
