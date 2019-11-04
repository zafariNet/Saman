using System;
using Model.Employees;
using Infrastructure.UnitOfWork;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
