using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;
using Model.Sales;

namespace Repository.Mapping
{
    public class CreditServiceMap : ClassMapping<CreditService>
    {
        public CreditServiceMap()
        {
            Table("Store.CreditService");

            // Base Properties
            Id(x => x.ID, c => c.Column("CreditServiceID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // CreditService Properties
            ManyToOne(x => x.Network, c => c.Column("NetworkID"));
            Property(x => x.ServiceName, c => c.Length(50));
            Property(x => x.CreditServiceCode);
            Property(x => x.UnitPrice);
            Property(x => x.PurchaseUnitPrice);
            Property(x => x.ResellerUnitPrice);
            Property(x => x.MaxDiscount);
            Property(x => x.Imposition);
            Property(x => x.Discontinued);
            Property(x => x.ExpDays);
            Property(x => x.Note);
            Property(x => x.Bonus);
            Property(x=>x.Comission);
            Property(x => x.SortOrder);


            //Bags
            Bag(x => x.CreditSaleDetails,
            collectionMapping =>
            {

                collectionMapping.Table("Sale.CreditSaleDetail");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("CreditServiceID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(CreditSaleDetail))));

        }
    }
}
