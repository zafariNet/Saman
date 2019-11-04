using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Employees;

namespace Repository.Mapping
{
    public class LocalPhoneMap : ClassMapping<LocalPhone>
    {
        public LocalPhoneMap()
        {
            Table("Emp.LocalPhone");

            // Base Properties
            Id(x => x.ID, c => c.Column("LocalPhoneID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // LocalPhone Properties
            Property(x => x.LocalPhoneNumber);
            ManyToOne(x => x.OwnerEmployee, c => c.Column("OwnerEmployeeID"));

        }
    }
}
