using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class QueryEmployeeMap : ClassMapping<QueryEmployee>
    {
        public QueryEmployeeMap()
        {
            Table("Cus.QueryEmployee");

            // Base Properties
            ComposedId(cid =>
            {
                cid.ManyToOne((x) => x.Query, (c) => c.Column("QueryID"));
                cid.ManyToOne((x) => x.Employee, (c) => c.Column("EmployeeID"));
            });
        }
    }
}
