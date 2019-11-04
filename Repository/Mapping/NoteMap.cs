using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NoteMap : ClassMapping<Note>
    {
        public NoteMap()
        {
            Table("Cus.Note");

            // Base Properties
            Id(x => x.ID, c => c.Column("NoteID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Note Properties
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            ManyToOne(x => x.Level, c => c.Column("LevelID"));
            Property(x => x.NoteDescription);
        }
    }
}
