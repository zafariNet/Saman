using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NetworkMap : ClassMapping<Network>
    {
        public NetworkMap()
        {
            Table("Store.Network");

            // Base Properties
            Id(x => x.ID, c => c.Column("NetworkID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            Property(x => x.SortOrder);
            // Network Properties
            Property(x => x.NetworkName, c => c.Length(30));
            Property(x => x.Note);
            Property(x => x.DeliverWhenCreditLow);
            Property(x => x.Discontinued, c => c.Column("Discontinued"));
            Property(x => x.Balance);
            Property(x=>x.Alias);

            //Bags
            //Bag(x => x.Customers,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Cus.Customer");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("NetworkID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
               //mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Customers.Customer))));

            Bag(x => x.NetworkCenters,
            collectionMapping =>
            {

                collectionMapping.Table("Cus.NetworkCenter");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.Remove);
                collectionMapping.Key(k => k.Column("NetworkID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Customers.NetworkCenter))));

            Bag(x => x.CreditServices,
            collectionMapping =>
            {

                collectionMapping.Table("Store.CreditService");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("NetworkID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(CreditService))));

            Bag(x => x.NetworkCredits,
            collectionMapping =>
            {
                collectionMapping.Table("Store.NetworkCredit");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("NetworkID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany());

  
        }
    }
}
