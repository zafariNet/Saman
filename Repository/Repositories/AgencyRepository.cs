using System;
using System.Collections.Generic;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;

namespace Repository.Repositories
{
    public class AgencyRepository : Repository<Agency>, IAgencyRepository
    {
        public AgencyRepository(IUnitOfWork uow)
            : base(uow)
        {
        }

        public void RemoveById(Guid id)
        {
            SessionFactory.GetCurrentSession().Delete<Agency>(id);
        }
    }
}
