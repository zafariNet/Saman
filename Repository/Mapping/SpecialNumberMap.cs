using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SpecialNumberMap : ClassMapping<SpecialNumber>
    {
        public SpecialNumberMap()
        {
            Table("Cus.SpecialNumber");

            // Base Properties
            Id(x => x.ID, c => c.Column("SpecialNumberID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // SpecialNumber Properties
            Property(x => x.FromNumber);
            Property(x => x.ToNumber);
            Property(x => x.Note);
        }
    }
}
