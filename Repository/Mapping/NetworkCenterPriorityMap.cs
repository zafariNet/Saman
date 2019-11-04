using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class NetworkCenterPriorityMap:ClassMapping<NetworkCenterPriority>
    {
        public NetworkCenterPriorityMap()
        {
            Table("Cus.NetworkCenterPriority");

            // Base Properties

            ManyToOne(x=>x.Center,c=>c.Column("CenterID"));
            ManyToOne(x=>x.Network,c=>c.Column("NetworkID"));
            Property(x=>x.SalePriority);
        }
    }
}
