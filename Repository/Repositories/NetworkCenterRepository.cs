using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Infrastructure.Querying;

namespace Repository.Repositories
{
    public class NetworkCenterRepository : Repository<NetworkCenter>, INetworkCenterRepository
    {
        public NetworkCenterRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
