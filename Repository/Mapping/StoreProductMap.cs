using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class StoreProductMap : ClassMapping<StoreProduct>
    {
        public StoreProductMap()
        {
            Table("Store.StoreProduct");

            // Base Properties
            ComposedId(cid =>
            {
                cid.ManyToOne((x) => x.Store, (c) => c.Column("StoreID"));
                cid.ManyToOne((x) => x.Product, (c) => c.Column("ProductID"));
            });
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            // StoreProduct Properties
            Property(x => x.UnitsInStock);
        }
    }
}
