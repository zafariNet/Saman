using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NetworkCreditMap : ClassMapping<NetworkCredit>
    {
        public NetworkCreditMap()
        {
            Table("Store.NetworkCredit");

            // Base Properties
            Id(x => x.ID, c => c.Column("NetworkCreditID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // NetworkCredit Properties
            ManyToOne(x => x.Network, c => c.Column("NetworkID"));
            ManyToOne(x => x.FromAccount, c => c.Column("FromAccountID"));
            Property(x => x.Amount);
            Property(x => x.InvestDate, c => c.Length(10));
            Property(x => x.ToAccount, c => c.Length(50));
            Property(x => x.TransactionNo, c => c.Length(50));
            Property(x => x.Note);
        }
    }
}