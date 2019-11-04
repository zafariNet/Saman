using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode.Impl;

namespace Repository.Mapping
{
    public class SmsEmployeeMap:ClassMapping<SmsEmployee>
    {
        public SmsEmployeeMap()
        {
            Table("Emp.SmsEmployee");
            // Base Properties
            Id(x => x.ID, c => c.Column("SmsEmployeeID"));
            Property(x => x.CreateDate, m => m.Length(19));

            Property(x=>x.Body);
            Property(x=>x.Note);
            ManyToOne(x=>x.Employee,c=>c.Column("OwnerEmployeeID"));
        }
    }
}
