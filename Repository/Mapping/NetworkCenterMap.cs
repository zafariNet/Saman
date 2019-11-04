using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NetworkCenterMap : ClassMapping<NetworkCenter>
    {
        public NetworkCenterMap()
        {
            Table("Cus.NetworkCenter");

            // Base Properties
            ComposedId(cid =>
            {
                cid.ManyToOne((x) => x.Network, (c) => c.Column("NetworkID"));
                cid.ManyToOne((x) => x.Center, (c) => c.Column("CenterID"));
            });
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            Property(x=>x.CanSale);

            // NetworkCenter Properties
            Property(x => x.Status);
        }
    }
}
