using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Infrastructure.UnitOfWork;
using Model.Store.Interfaces;

namespace Repository.Repositories
{
    public class ProductLogRepository : Repository<ProductLog>, IProductLogRepository
    {
        public ProductLogRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
