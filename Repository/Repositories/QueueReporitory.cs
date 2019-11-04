using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Repositories
{
    public class QueueReporitory : Repository<Queue>, IQueueRepository
    {
        public QueueReporitory(IUnitOfWork uow):base(uow)
        {

        }
    }
}
