using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class CenterMap : ClassMapping<Center>
    {
        public CenterMap()
        {
            Table("Cus.Center");

            // Base Properties
            Id(x => x.ID, c => c.Column("CenterID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Center Properties
            Property(x => x.CenterName, c => c.Length(40));
            Property(x => x.Note);
            //Property(x => x.CustomerCount, c => c.Formula(@"(Select Count(*) From Cus.Customer c Where c.CenterID = CenterID)"));

            Bag(x => x.NetworkCenters,
            collectionMapping =>
            {
                collectionMapping.Table("Cus.NetworkCenter");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Key(k => k.Column("CenterID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Customers.NetworkCenter))));
        }
    }
}
