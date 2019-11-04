using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CodeMap : ClassMapping<Code>
    {
        public CodeMap()
        {
            Table("Cus.Code");

            // Base Properties
            Id(x => x.ID, c => c.Column("CodeID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Code Properties
            Property(x => x.CodeName, c => c.Length(5));
            ManyToOne(x => x.Center, c => c.Column("CenterID"));
            Property(x=>x.AddedToSite);
        }
    }
}
