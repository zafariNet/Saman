using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    class ToDoResultMap:ClassMapping<ToDoResult>
    {
        public ToDoResultMap()
        {
            Table("Emp.ToDoResult");

            // Base Properties
            Id(x => x.ID, c => c.Column("ToDoResultID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            Property(x => x.Attachment, c => c.Column("Attachment"));
            Property(x => x.AttachmentType, c => c.Column("AttachmentType"));
            ManyToOne(x => x.ReferedEmployee, c => c.Column("ReferedEmployeeID"));
            ManyToOne(x => x.ToDo, c => c.Column("ToDoID"));

            Property(x => x.ToDoResultDescription, c => c.Column("ToDoResultDescription"));
            Property(x => x.RemindeTime, c => c.Column("RemindeTime"));
            Property(x => x.SecondaryClosed, c => c.Column("SecondaryClosed"));
            Property(x=>x.Remindable,c=>c.Column("Remindable"));
        }
    }
}