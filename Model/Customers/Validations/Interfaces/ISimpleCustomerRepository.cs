﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;

namespace Model.Customers.Validations.Interfaces
{
    public interface ISimpleCustomerRepository : IRepository<SimpleCustomer>
    {
        IList<SimpleCustomer> FindSimpleCustomer(string ADSLPhone);
    }
}
