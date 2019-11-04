using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class ProductMap : ClassMapping<Product>
    {
        public ProductMap()
        {
            Table("Store.Product");

            // Base Properties
            Id(x => x.ID, c => c.Column("ProductID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Product Properties
            ManyToOne(x => x.ProductCategory, c => c.Column("ProductCategoryID"));
            Property(x => x.ProductName, c => c.Length(50));
            Property(x => x.ProductCode);
            Property(x => x.UnitsInStock);
            Property(x => x.Discontinued);
            Property(x => x.Note);
            Property(x => x.SortOrder);
            

            //Bags
            //Bag(x => x.ProductSaleDetails,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Sales.ProductSaleDetail");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("ProductID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Sales.ProductSaleDetail))));

            //Bag(x => x.ProductDeliverys,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Store.ProductDelivery");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("ProductID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(ProductDelivery))));

            Bag(x => x.ProductLogs,
            collectionMapping =>
            {

                collectionMapping.Table("Store.ProductLog");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("ProductID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(ProductLog))));

            //Bag(x => x.ProductPrices,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Store.ProductPrice");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("ProductID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(ProductPrice))));

            Bag(x => x.StoreProducts,
            collectionMapping =>
            {

                collectionMapping.Table("Store.StoreProduct");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("ProductID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(StoreProduct))));
        }
    }
}
