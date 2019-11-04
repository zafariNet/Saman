using System;
using System.Collections.Generic;
using Infrastructure.Domain;
using Model.Sales;
using Infrastructure.UnitOfWork;
using NHibernate;
using Model.Sales.Interfaces;
using System.Linq;

namespace Repository.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public IEnumerable<Sale> FindAll(Guid CustomerID)
        {
            IEnumerable<Sale> allSales = FindAll();

            return allSales.Where(w => w.Customer.ID == CustomerID);
        }

    }
}
