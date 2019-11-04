using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class PermitMap : ClassMapping<Permit>
    {
        public PermitMap()
        {
            Table("Emp.Permit");

            // Permit Properties
            Id(x => x.ID, c => c.Column("PermitID"));
            Property(x => x.Guaranteed);
            ManyToOne(x => x.Permission, c => c.Column("PermissionID"));
            ManyToOne(x => x.Employee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.Group, c => c.Column("GroupID"));
        }
    }
}
