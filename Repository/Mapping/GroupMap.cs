using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Employees;

namespace Repository.Mapping
{
    public class GroupMap : ClassMapping<Group>
    {
        public GroupMap()
        {
            Table("Emp.[Group]");

            // Base Properties
            Id(x => x.ID, c => c.Column("GroupID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            ManyToOne(x => x.ParentGroup, c => c.Column("ParentGroupID"));
            ManyToOne(x => x.GroupStaff, c => c.Column("GroupStaffID"));
            Property(x => x.RowVersion);

            // Group Properties
            Property(x => x.GroupName, m => m.Length(50));
            //Property(x => x.Permissions);
            //Bag<Employee>(x => x.Employees, cp => { }, 
            //    cr => cr.OneToMany(x => x.Class(typeof(Employee))));

            //Bags

            Bag(x => x.Employees,
            collectionMapping =>
            {

                collectionMapping.Table("Emp.Employee");
                //collectionMapping.Access(typeof(long));
                //collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.Detach);
                collectionMapping.Key(k => k.Column("GroupID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Employees.Employee))));

            Bag(x => x.Permissions,
            collectionMapping =>
            {

                collectionMapping.Table("Emp.Permit");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Key(k => k.Column("GroupID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Employees.Permit))));

            //Bag(x => x.Tasks,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Task.Task");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("GroupID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //    mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Tasks.Task))));
        }
    }
}
