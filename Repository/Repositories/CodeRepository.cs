using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;

namespace Repository.Repositories
{
    public class CodeRepository : Repository<Code>, ICodeRepository
    {
        public CodeRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
