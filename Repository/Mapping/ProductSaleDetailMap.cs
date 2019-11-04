using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Store;

namespace Repository.Mapping
{
    public class ProductSaleDetailMap : ClassMapping<ProductSaleDetail>
    {
        public ProductSaleDetailMap()
        {
            Table("Sales.ProductSaleDetail");

            // Base Properties
            Id(x => x.ID, c => c.Column("ProductSaleDetailID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // ProductSaleDetail Properties
            ManyToOne(x => x.Sale, c => c.Column("SaleID"));
            ManyToOne(x => x.ProductPrice, c => c.Column("ProductPriceID"));
            Property(x => x.UnitPrice);
            Property(x => x.Units);
            Property(x => x.Discount);
            Property(x => x.Imposition);
            Property(x => x.RollbackNote);
            Property(x => x.RollbackPrice);
            Property(x => x.IsRollbackDetail);

            ManyToOne(x => x.DeliverEmployee, m => m.Column("DeliverEmployeeID"));
            Property(x => x.DeliverNote);
            Property(x => x.DeliverDate);
            ManyToOne(x => x.DeliverStore, m => m.Column("DeliverStoreID"));

            Property(x => x.LineDiscount);
            Property(x => x.LineImposition);
            Property(x => x.LineTotal);
            Property(x => x.LineTotalWithoutDiscountAndImposition);
            Property(x => x.Delivered);
            Property(x => x.Status);
            Property(x => x.Rollbacked);
            ManyToOne(x => x.MainSaleDetail, c => c.Column("MainSaleDetailID"));
            ManyToOne(x => x.RollbackedProductSaleDetail, c => c.Column("RollbakedProductSaleDetailID"));
            ManyToOne(x => x.BonusComission, c => c.Column("BonusComissionID"));

            Property(x => x.UnDeliveredComission);
            Property(x => x.Comission);
            Property(x => x.Bonus);
            Property(x => x.BonusDate);
            Property(x => x.ComissionDate);
            ManyToOne(x => x.SaleEmployee, m => m.Column("SaleEmployeeID"));
            
            // Bags


            //             RollbackedCreditSaleDetails
            //Bag(x => x.RollbackedProductSaleDetails,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Sales.ProductSaleDetail");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.All);
            //    collectionMapping.Key(k => k.Column("MainProductSaleDetailID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(ProductSaleDetail))));

        }
    }
}
