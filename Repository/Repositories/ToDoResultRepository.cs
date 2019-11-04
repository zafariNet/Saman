using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Repositories
{
    public class ToDoResultRepository : Repository<ToDoResult>, IToDoResultRepository
    {
        public ToDoResultRepository(IUnitOfWork uow)
            :base(uow)
        {

        }
    }
}
