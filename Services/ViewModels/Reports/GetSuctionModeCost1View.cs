using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class GetSuctionModeCost1View
    {
        public string PaymentDate { get; set; }
        public string CampaignAgentName { get; set; }
        public string SuctionModeName { get; set; }
        public string SuctionModeDetailName { get; set; }
        public long Amount { get; set; }
    }

    public class GetSuctionModeCost2View
    {
        public Guid ID { get; set; }
        public string CampaignAgentName { get; set; }
        public Guid CampaignAgentID { get; set; }
        public long Amount { get; set; }
    }
}
