using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class TaskRepository:Repository<Task>,ITaskRepository
    {
        public TaskRepository(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}
