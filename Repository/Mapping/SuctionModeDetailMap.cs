using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class SuctionModeDetailMap:ClassMapping<SuctionModeDetail>
    {
        public SuctionModeDetailMap()
        {
            Table("Cus.SuctionModeDetail");
            Id(x => x.ID, c => c.Column("SuctionModeDetailID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            ManyToOne(x => x.SuctionMode, c => c.Column("SuctionModeID"));
            Property(x => x.RowVersion);
            Property(x => x.SortOrder);
            Property(x => x.Discontinued);
            Property(x => x.SuctionModeDetailName, c => c.Column("SuctionModeDetailName"));
            
            //Bag(x => x.Customers,
            //    collectionMapping =>
            //    {

            //        collectionMapping.Table("Cus.Customer");
            //        //collectionMapping.Access(typeof(long));
            //        collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //        collectionMapping.Key(k => k.Column("SuctionModeDetailID"));
            //        collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //    },
            //    mapping => mapping.OneToMany(cr => cr.Class(typeof(Customer))));

        }
    }
}
