using Model.Employees;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Mapping
{
    public class SimpleEmployeeMap : ClassMapping<SimpleEmployee>
    {
        public SimpleEmployeeMap()
        {
            Table("Emp.Employee");

            // Base Properties
            Id(x => x.ID, c => c.Column("EmployeeID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            ManyToOne(x => x.ParentEmployee, c => c.Column("ParentEmployeeID"));
            Property(x => x.LastName, c => c.Length(25));
            Property(x => x.FirstName, c => c.Length(20));
            ManyToOne(x => x.Group, c => c.Column("GroupID"));

            Bag(x => x.ChildEmployees,
collectionMapping =>
{

    collectionMapping.Table("Emp.Employee");
    //collectionMapping.Access(typeof(long));
    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
    collectionMapping.Key(k => k.Column("ParentEmployeeID"));
    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
},
   mapping => mapping.OneToMany(cr => cr.Class(typeof(SimpleEmployee))));
        }
    }
}
