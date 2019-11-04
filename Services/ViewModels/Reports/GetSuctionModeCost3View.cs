using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Support;

namespace Services.ViewModels.Reports
{
    public class GetSuctionModeCost3View
    {
        public Guid ID { get { return Guid.NewGuid(); } }
        public long CostAmount { get; set; }
        public Guid suctionModeID { get; set; }
        public string SuctionModeName { get; set; }
        public string SuctionModeDetailName { get; set; }
        public Guid SuctionModeDetailID { get; set; }
        public int UserCountPerCenter { get; set; }
        public long CostPerUserSuctionMode {
            get
            {
                if (this.CostAmount != 0 && UserCountPerSuctionModeDetal!=0)
                return CostAmount/UserCountPerSuctionModeDetal;
                return 0;
            }
        }
        public long CostPerUserSuctionCenter {
            get
            {
                return UserCountPerCenter*CostPerUserSuctionMode;
            }
        }
        public string CenterName { get; set; }
        public int UserCountPerSuctionModeDetal { get; set; }
    }

    public class GetcampaignAgents
    {
        public string PaymentDate { get; set; }
        public Guid SuctionModeDetailID { get; set; }
        public Guid SuctionModeID { get; set; }
        public string SuctionModeDetailName { get; set; }
        public string SuctionMoedName { get; set; }
        public long Amount { get; set; }
    }

    public class GetCustomerCampaignView
    {
        public Guid CenterID { get; set; }
        public string CenterName { get; set; }
        public Guid SuctionModeDetailID { get; set; }
        public Guid SuctionModeID { get; set; }
        public string SuctionModeDetailName { get; set; }
        public string SuctionMoedName { get; set; }
        public string IsRunje { get; set; }
        public string InputSupporttDate { get; set; }
        public bool HasFiscal { get; set; }
        public SupportView Supports { get; set; }
    }
}
