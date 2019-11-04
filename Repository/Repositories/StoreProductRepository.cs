using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Infrastructure.UnitOfWork;
using Model.Store.Interfaces;
using NHibernate;
using Infrastructure.Querying;

namespace Repository.Repositories
{
    public class StoreProductRepository : Repository<StoreProduct>, IStoreProductRepository
    {
        public StoreProductRepository(IUnitOfWork uow)
            : base(uow)
        {
        }

        public StoreProduct FindBy(Guid StoreID, Guid ProductID)
        {
            Query query = new Query();
            Criterion criterion1 = new Criterion("Store.ID", StoreID, CriteriaOperator.Equal);
            Criterion criterion2 = new Criterion("Product.ID", ProductID, CriteriaOperator.Equal);

            query.Add(criterion1);
            query.Add(criterion2);

            return FindBy(query).FirstOrDefault();
        }
    }
}
