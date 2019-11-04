using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;

namespace Repository.Repositories
{
    public class QueryRepository : Repository<Query>, IQueryRepository
    {
        public QueryRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
