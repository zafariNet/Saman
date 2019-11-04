using System;
using System.Collections.Generic;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Customers.Validations.Interfaces;

namespace Repository.Repositories
{
    public class SuctionModeDetailRepository:Repository<SuctionModeDetail>,ISuctionModeDetailRepository
    {
        public SuctionModeDetailRepository(IUnitOfWork uow) 
            :base(uow)
        {

        }
    }
}
