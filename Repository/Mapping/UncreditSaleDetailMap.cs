using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Store;

namespace Repository.Mapping
{
    public class UncreditSaleDetailMap : ClassMapping<UncreditSaleDetail>
    {
        public UncreditSaleDetailMap()
        {
            Table("Sales.UncreditSaleDetail");

            // Base Properties
            Id(x => x.ID, c => c.Column("UncreditSaleDetailID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // UncreditSaleDetail Properties
            ManyToOne(x => x.Sale, c => c.Column("SaleID"));
            ManyToOne(x => x.UncreditService, c => c.Column("UncreditServiceID"));
            Property(x => x.UnitPrice);
            Property(x => x.Units);
            Property(x => x.Discount);
            Property(x => x.Imposition);
            Property(x => x.RollbackNote);
            Property(x => x.RollbackPrice);
            ManyToOne(x => x.DeliverEmployee, m => m.Column("DeliverEmployeeID"));
            Property(x => x.DeliverNote);
            Property(x => x.DeliverDate);
            Property(x => x.IsRollbackDetail);
            ManyToOne(x => x.BonusComission, c => c.Column("BonusComissionID"));

            Property(x=>x.UnDeliveredComission);
            Property(x=>x.Comission);
            Property(x=>x.Bonus);
            Property(x=>x.BonusDate);
            Property(x=>x.ComissionDate);
            ManyToOne(x => x.SaleEmployee, m => m.Column("SaleEmployeeID"));

            //#region New changes

            Property(x => x.LineDiscount);
            Property(x => x.LineImposition);
            Property(x => x.LineTotal);
            Property(x => x.LineTotalWithoutDiscountAndImposition);
            Property(x => x.Delivered);
            Property(x => x.Status);
            ManyToOne(x => x.MainSaleDetail, c => c.Column("MainSaleDetailID"));
            ManyToOne(x => x.RollbackedUncreditSaleDetail, c => c.Column("RollbackedUncreditSaleDetailID"));
            //Property(x => x.RowVersion);
            //Property(x => x.RowVersion);
            //Property(x => x.CanDeliver);
            //Property(x => x.CanRollback);
            //Property(x => x.IsRollbackDetail);
            Property(x => x.Rollbacked);
            //Property(x=>x.RollbackedUncreditSaleDetail);
            //Property(x=>x.MainUnCreditsaleDetail);
            //#endregion

            // RollbackedCreditSaleDetails
            //Bag(x => x.RollbackedUncreditSaleDetails,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Sales.UncreditSaleDetail");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.None);
            //    collectionMapping.Key(k => k.Column("MainUncreditSaleDetailID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(UncreditSaleDetail))));

            //Bag(x => x.MainUnCreditsaleDetails,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Sales.UncreditSaleDetail");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.None);
            //    collectionMapping.Key(k => k.Column("MainUncreditSaleDetailID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(UncreditSaleDetail))));

        }
    }
}
