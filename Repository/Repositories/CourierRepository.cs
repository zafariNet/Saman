using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Sales;
using Model.Sales.Interfaces;

namespace Repository.Repositories
{
    public class CourierRepository:Repository<Courier>,ICourierRepository
    {
        public CourierRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
