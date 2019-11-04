using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    public class ToDoMap : ClassMapping<ToDo>
    {
        public ToDoMap()
        {
            Table("Emp.ToDo");

            // Base Properties
            Id(x => x.ID, c => c.Column("ToDoID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));

            Property(x => x.ToDoTitle, c => c.Column("ToDoTitle"));
            Property(x => x.ToDoDescription, c => c.Column("ToDoDescription"));
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            Property(x => x.StartDate, c => c.Column("StartDate"));
            Property(x => x.EndDate, c => c.Column("EndDate"));
            Property(x => x.StartTime, c => c.Column("StartTime"));
            Property(x => x.EndTime, c => c.Column("EndTime"));
            Property(x => x.PriorityType, c => c.Column("PriorityType"));
            Property(x => x.PrimaryClosed, c => c.Column("PrimaryClosed"));
            Property(x => x.IsGrouped, c => c.Column("isGrouped"));
            Property(x => x.RowVersion);
            Property(x => x.Attachment, c => c.Column("Attachment"));
            Property(x => x.AttachmentType, c => c.Column("AttachmentType"));
            Bag(x => x.ToDoResults,
                collectionMapping =>
                {
                    collectionMapping.Table("Emp.ToDoResult");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                    collectionMapping.Key(k => k.Column("ToDoID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                    mapping => mapping.OneToMany(cr => cr.Class(typeof(ToDoResult))));

        }
    }
}
