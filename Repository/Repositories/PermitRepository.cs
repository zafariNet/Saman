﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Infrastructure.UnitOfWork;
using Model.Employees.Interfaces;

namespace Repository.Repositories
{
    public class PermitRepository : Repository<Permit>, IPermitRepository
    {
        public PermitRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
