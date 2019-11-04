using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Infrastructure.UnitOfWork;
using Model.Fiscals.Interfaces;

namespace Repository.Repositories
{
    public class MoneyAccountEmployeeRepository : Repository<MoneyAccountEmployee>, IMoneyAccountEmployeeRepository
    {
        public MoneyAccountEmployeeRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
