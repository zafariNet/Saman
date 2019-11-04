using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CustomerLevelMap : ClassMapping<CustomerLevel>
    {
        public CustomerLevelMap()
        {
            Table("Cus.CustomerLevel");

            // Base Properties
            Id(x => x.ID, c => c.Column("CustomerLevelID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // CustomerLevel Properties
            ManyToOne(x => x.Level, c => c.Column("LevelID"));
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            Property(x => x.Note);
        }
    }
}
