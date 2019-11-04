using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class UncreditServiceMap : ClassMapping<UncreditService>
    {
        public UncreditServiceMap()
        {
            Table("Store.UncreditService");
 
            // Base Properties
            Id(x => x.ID, c => c.Column("UncreditServiceID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // UncreditService Properties
            Property(x => x.UncreditServiceName, c => c.Length(50));
            Property(x => x.UnCreditServiceCode);
            Property(x => x.UnitPrice);
            Property(x => x.MaxDiscount);
            Property(x => x.Imposition);
            Property(x => x.Discontinued);
            Property(x => x.SortOrder);
            Property(x=>x.Comission);
            Property(x => x.Bonus);
            Property(x => x.Note);
            

            //Bags
            //Bag(x => x.Customers,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Cus.Customer");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("UncreditServiceID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Customers.Customer))));

            // CreditServiceDeliverys

            Bag(x => x.UncreditSaleDetails,
           collectionMapping =>
           {

               collectionMapping.Table("Sales.UncreditSaleDetail");
               //collectionMapping.Access(typeof(long));
               collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
               collectionMapping.Key(k => k.Column("UncreditServiceID"));
               collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
           },
              mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Sales.UncreditSaleDetail))));
        }
    }
}
