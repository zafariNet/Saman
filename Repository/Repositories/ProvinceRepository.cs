﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;

namespace Repository.Repositories
{
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(IUnitOfWork uow)
            : base(uow)
        {

        }
    }
}
