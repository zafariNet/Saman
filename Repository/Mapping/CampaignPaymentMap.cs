using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Model.Sales;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    class CampaignPaymentMap:ClassMapping<CampaignPayment>
    {
        public CampaignPaymentMap()
        {
            Table("Sales.CampaignPayment");

            // Base Properties
            Id(x => x.ID, c => c.Column("CampaignPaymentID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            ManyToOne(x => x.CampaignAgent, c => c.Column("CampaignAgentID"));
            ManyToOne(x => x.SuctionModeDetail, c => c.Column("SuctionModeDetalID"));
            Property(x=>x.Amount);
            Property(x=>x.PaymentDate);
            Property(x=>x.Note);
        }
    }
}
