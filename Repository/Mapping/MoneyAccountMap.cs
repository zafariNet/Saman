using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class MoneyAccountMap : ClassMapping<MoneyAccount>
    {
        public MoneyAccountMap()
        {
            Table("Fiscal.MoneyAccount");

            // Base Properties
            Id(x => x.ID, c => c.Column("MoneyAccountID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            Property(x => x.SortOrder);
            // MoneyAccount Properties
            //ManyToOne(x => x.AccountEmployees, c => c.Column("AccountEmployeesID"));
            Property(x => x.AccountName, c => c.Length(50));
            Property(x => x.Receipt);
            Property(x => x.Pay);
            Property(x => x.IsBankAccount);
            Property(x => x.FiscalPrefix);
            Property(x => x.AccountingSoftwareID);
            Property(x => x.BAccountNumber, c => c.Length(30));
            Property(x => x.BAccountInfo, c => c.Length(250));
            Property(x => x.Discontinued, c => c.Column("Discontinued"));
            Property(x => x.HasUniqueSerialNumber, c => c.Column("HasUniqueSerialNumber"));
            Bag(x => x.MoneyAccountEmployees,
            collectionMapping =>
            {

                collectionMapping.Table("Fiscal.MoneyAccountEmployee");
                //collectionMapping.Access(typeof(long));
                collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                collectionMapping.Key(k => k.Column("MoneyAccountID"));
                collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            },
               mapping => mapping.OneToMany(cr => cr.Class(typeof(MoneyAccountEmployee))));
            Property(x=>x.Has9Digits);

            //Bag(x => x.Networks,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Store.Network");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("MoneyAccountID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(Model.Store.Network))));
        }
    }
}
