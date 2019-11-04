using Model.Sales;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;
using System;

namespace Repository.Mapping
{
    public class SaleMap : ClassMapping<Sale>
    {
        public SaleMap()
        {
            Table("Sales.Sale");

            #region Base Properties
            Id(x => x.ID, c => c.Column("SaleID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            #endregion

            #region Sale Properties

            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            ManyToOne(x => x.MainSale, c => c.Column("MainSaleID"));
            Property(x => x.SaleNumber, c => c.Length(10));
            Property(x => x.Closed);
            ManyToOne(x => x.CloseEmployee, c => c.Column("CloseEmployeeID"));
            Property(x => x.CloseDate, c => c.Length(19));
            ManyToOne(x=>x.Couriers,c=>c.Column("CourierID"));
            Property(x => x.HasCourier);
            
            
            #endregion

            #region Bags

            //Courier

            //Bag(x => x.Couriers,
            //    collectionMapping =>
            //    {
            //        collectionMapping.Table("Sales.Courier");
            //        collectionMapping.Access(typeof(long));
            //        collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
            //        collectionMapping.Inverse(true);
            //        collectionMapping.Key(k => k.Column("SaleID"));
            //        collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //    },
            //    mapping => mapping.OneToMany(cr => cr.Class(typeof(Courier))));

             //CreditSaleDetails
            Bag(x => x.CreditSaleDetails,
            collectionMapping =>
            {
                collectionMapping.Table("Sales.CreditSaleDetail");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Inverse(true);
                collectionMapping.Key(k => k.Column("SaleID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(CreditSaleDetail))));

            // UncreditSaleDetail
            Bag(x => x.UncreditSaleDetails,
            collectionMapping =>
            {
                collectionMapping.Table("Sales.UncreditSaleDetail");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Inverse(true);
                collectionMapping.Key(k => k.Column("SaleID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(UncreditSaleDetail))));

            // ProductSaleDetail
            Bag(x => x.ProductSaleDetails,
            collectionMapping =>
            {
                collectionMapping.Table("Sales.ProductSaleDetail");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
                collectionMapping.Inverse(true);
                collectionMapping.Key(k => k.Column("SaleID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(ProductSaleDetail))));

            // ChildSales
            Bag(x => x.RollbackedSales,
            collectionMapping =>
            {
                collectionMapping.Table("Sales.Sale");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.None);
                //collectionMapping.Inverse(true);
                collectionMapping.Key(k => k.Column("MainSaleID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Sale))));

            #endregion
        }
    }
}