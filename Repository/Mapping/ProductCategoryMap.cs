using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class ProductCategoryMap : ClassMapping<ProductCategory>
    {
        public ProductCategoryMap()
        {
            Table("Store.ProductCategory");

            // Base Properties
            Id(x => x.ID, c => c.Column("ProductCategoryID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // ProductCategory Properties
            Property(x => x.ProductCategoryName, c => c.Length(100));
            Property(x => x.Note);
            Property(x => x.Discontinued);

            //Bags
            Bag(x => x.Products,
            collectionMapping =>
            {

                collectionMapping.Table("Store.Product");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("ProductCategoryID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Product))));
        }
    }
}
