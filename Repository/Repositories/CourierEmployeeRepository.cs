using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class CourierEmployeeRepository:Repository<CourierEmployee>,ICourierEmployeeRepository
    {
        public CourierEmployeeRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
