using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class FollowStatusMap : ClassMapping<FollowStatus>
    {
        public FollowStatusMap()
        {
            Table("Cus.FollowStatus");

            // Base Properties
            Id(x => x.ID, c => c.Column("FollowStatusID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // SuctionMode Properties
            Property(x => x.FollowStatusName, c => c.Length(100));
            Property(x => x.SortOrder);

        //    //Bags
        //    Bag(x => x.Customers,
        //    collectionMapping =>
        //    {

        //        collectionMapping.Table("Cus.Customer");
        //        //collectionMapping.Access(typeof(long));
        //        collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
        //        collectionMapping.Key(k => k.Column("FollowStatusID"));
        //        collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
        //    },
        //       mapping => mapping.OneToMany(cr => cr.Class(typeof(Customer))));
        }
    }
}
