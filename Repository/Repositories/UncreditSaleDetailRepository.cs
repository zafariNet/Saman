using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using Infrastructure.UnitOfWork;
using Model.Sales.Interfaces;
using NHibernate;

namespace Repository.Repositories
{
    public class UncreditSaleDetailRepository : Repository<UncreditSaleDetail>, IUncreditSaleDetailRepository
    {
        public UncreditSaleDetailRepository(IUnitOfWork uow)
            : base(uow)
        {

        }

    }
}
