using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Repositories
{
    public class QueueLocalPhoneStoreRepository : Repository<QueueLocalPhoneStore>, IQueueLocalPhoneStoreRepository
    {
        public QueueLocalPhoneStoreRepository(IUnitOfWork uow):base(uow)
        {

        }
    }
}
