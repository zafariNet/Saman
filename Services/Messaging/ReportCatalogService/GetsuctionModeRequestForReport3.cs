using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.ReportCatalogService
{
    public class GetsuctionModeRequestForReport3
    {
        public string PaymentStartDate { get; set; }
        public string PaymentEndDate { get; set; }
        public IEnumerable<Guid?> CenterIDs { get; set; }
        public IEnumerable<Guid?> SuctionModeDetailsIDs { get; set; }
        public IEnumerable<Guid> SuctionModeIDs { get; set; }
        public string RegisterStartDate { get; set; }
        public string RegisterEndDate { get; set; }
        public bool IsRanje { get; set; }
        public string SupportInputStartDate { get; set; }
        public string SupportInputEndDate { get; set; }
        public bool HasFiscal { get; set; }
    
    }
}
