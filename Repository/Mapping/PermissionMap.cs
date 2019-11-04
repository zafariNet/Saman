using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class PermissionMap : ClassMapping<Permission>
    {
        public PermissionMap()
        {
            Table("Emp.Permission");

            // Base Properties
            Id(x => x.ID, c => c.Column("PermissionID"));

            // Permission Properties
            Property(x => x.Title, c => c.Length(100));
            Property(x => x.Group, c => c.Length(50));
            Property(x => x.Key, c => c.Length(50));
        }
    }
}
