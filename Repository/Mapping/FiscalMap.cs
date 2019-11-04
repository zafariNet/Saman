using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace Repository.Mapping
{
    public class FiscalMap : ClassMapping<Fiscal>
    {
        public FiscalMap()
        {
            Table("Fiscal.Fiscal");
            
            // Base Properties
            Property(x => x.FollowNumber, map => { map.Update(false); });

            //Id(x => x.FollowNumber, c => { c.Generator(Generators.Assigned); });
            Id(x => x.ID, c => c.Column("FiscalID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.AccountingSerialNumber, c => c.Column("AccountingSerialNumber"));
            Property(x => x.RowVersion);

            // Fiscal Properties
            ManyToOne(x => x.ConfirmEmployee, c => c.Column("ConfirmEmployeeID"));
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            ManyToOne(x => x.MoneyAccount, c => c.Column("MoneyAccountID"));
            Property(x => x.DocumentSerial, c => c.Length(20));
            Property(x => x.DocumentType);
            Property(x => x.Cost);
            Property(x => x.InvestDate, c => c.Length(10));
            Property(x => x.Note);
            Property(x => x.Confirm);
            Property(x => x.ConfirmedCost);
            Property(x => x.ConfirmDate, c => c.Length(19));
            Property(x => x.ChargeStatus);
            Property(x => x.SerialNumber);
            Property(x => x.ForCharge);
            Property(x => x.FiscalReciptNumber, c => c.Column("FiscalReciptNumber"));

            //Bags
            //Bag(x => x.MoneyAccount,
            //collectionMapping =>
            //{

            //    collectionMapping.Table("Fiscal.MoneyAccount");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.None);
            //    collectionMapping.Key(k => k.Column("MoneyAccountID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany(cr => cr.Class(typeof(MoneyAccount))));

        }
    }
}
