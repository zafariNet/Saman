using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SimpleCustomerMap:ClassMapping<SimpleCustomer>
    {
        public SimpleCustomerMap()
        {

            Id(x => x.CustomerID);

            Property(x => x.ADSLPhone);
            Property(x => x.LastName);
            Property(x => x.FirstName);
            Property(x => x.LevelTitle);
        }
    }
}
