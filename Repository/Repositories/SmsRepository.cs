using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;

namespace Repository.Repositories
{
    public class SmsRepository : Repository<Sms>, ISmsRepository
    {
        public SmsRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
