using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class AgencyMap : ClassMapping<Agency>
    {
        public AgencyMap()
        {
            Table("Cus.Agency");

            // Base Properties
            Id(x => x.ID, c => c.Column("AgencyID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Agency Properties
            Property(x => x.AgencyName, c => c.Length(30));
            Property(x => x.ManagerName, c => c.Length(50));
            Property(x => x.Phone1, c => c.Length(15));
            Property(x => x.Phone2, c => c.Length(15));
            Property(x => x.Mobile, c => c.Length(15));
            Property(x => x.Address);
            Property(x => x.Note);
            Property(x => x.SortOrder);
            Property(x => x.Discontinued);
            //Property(x => x.CustomerCount, c => c.Formula(@"(Select Count(*) From Cus.Customer c Where c.AgencyID = AgencyID)"));


            //Bags
            //Bag(x => x.Customers,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Customer");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("AgencyID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Customer))));

        }
    }
}
