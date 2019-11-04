using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Customers;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class NetworkCenterPriorityRepository:Repository<NetworkCenterPriority>,INetworkCenterPriorityRepository
    {
        public NetworkCenterPriorityRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
