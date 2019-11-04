using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class TaskMap:ClassMapping<Task>
    {
        public TaskMap()
        {
            Table("emp.Task");

            // Base Properties
            Id(x => x.ID, c => c.Column("TaskID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            Property(x => x.ToDoTitle);
            Property(x => x.ToDoDescription);
            Property(x => x.IsMaster);
            Property(x => x.PrimaryClosed);
            Property(x => x.SecondaryClosed);
            Property(x => x.PrimaryClosedDate);
            Property(x => x.SecondaryClosedDate);
            Property(x => x.ToDoResultDescription);
            Property(x=>x.SecondaryFile);
            Property(x => x.PrimaryFile);
            Property(x=>x.StartDate);
            Property(x=>x.EndDate);
            Property(x=>x.Reminder);
            Property(x=>x.RemindTime);
            Property(x=>x.SendSms);
            Property(x=>x.StartTime);
            Property(x=>x.EndTime);
            ManyToOne(x => x.ToDo, c => c.Column("MasterTaskID"));
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            ManyToOne(x => x.ReferedEmployee, c => c.Column("ReferedEmployeeID"));


            //Bags
            Bag(x => x.ToDoResults,
                collectionMapping =>
                {
                    collectionMapping.Table("Emp.Task");
                    //collectionMapping.Access(typeof(long));
                    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("MasterTaskID"));
                    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
                },
                mapping => mapping.OneToMany(cr => cr.Class(typeof(Task))));
        }
    }
}
