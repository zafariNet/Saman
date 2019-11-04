using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using Infrastructure.UnitOfWork;
using Model.Sales.Interfaces;

namespace Repository.Repositories
{
    public class ProductSaleDetailRepository : Repository<ProductSaleDetail>, IProductSaleDetailRepository
    {
        public ProductSaleDetailRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
