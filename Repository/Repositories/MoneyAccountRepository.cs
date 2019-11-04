using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Infrastructure.UnitOfWork;
using Model.Fiscals.Interfaces;

namespace Repository.Repositories
{
    public class MoneyAccountRepository : Repository<MoneyAccount>, IMoneyAccountRepository
    {
        public MoneyAccountRepository(IUnitOfWork uow)
            : base(uow)
        {
        }

    }
}
