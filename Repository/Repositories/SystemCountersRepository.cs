using Infrastructure.UnitOfWork;
using Model;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Repositories
{
    public class SystemCountersRepository : Repository<SystemCounters> ,ISystemCountersRepository
    {
        public SystemCountersRepository(IUnitOfWork uow):base(uow)
        {

        }
    }
}
        