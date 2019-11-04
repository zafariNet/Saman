using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Infrastructure.UnitOfWork;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
