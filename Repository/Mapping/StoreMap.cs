using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class StoreMap : ClassMapping<Store>
    {
        public StoreMap()
        {
            Table("Store.Store");

            // Base Properties
            Id(x => x.ID, c => c.Column("StoreID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Store Properties
            ManyToOne(x => x.OwnerEmployee, c => c.Column("OwnerEmployeeID"));
            Property(x => x.StoreName, c => c.Length(200));
            Property(x => x.Note);

            //Bags
            Bag(x => x.StoreProducts,
            collectionMapping =>
            {

                collectionMapping.Table("Store.StoreProduct");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("StoreID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(StoreProduct))));

        }
    }
}
