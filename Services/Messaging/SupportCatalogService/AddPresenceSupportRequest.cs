using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class AddPersenceSupportRequest
    {
        public Guid CustomerID { get; set; }
        /// <summary>
        /// نوع خدمات حضوری 
        ///1: نصب اولیه
        ///2: پشتیبانی حضوری
        /// </summary>
        public short SupportType { get; set; }
        public string Problem { get; set; }
        public string PlanDate { get; set; }
        public string PlanTimeFrom { get; set; }
        public string PlanTimeTo { get; set; }
        public string PlanNote { get; set; }
        public Guid InstallerID { get; set; }
        public bool Delivered { get; set; }
        public Int64 ReceivedCost { get; set; }
        public Int64 ReceivedCostForExtraServices { get; set; }
        public bool ConnectedToInternet { get; set; }
        public string DeliverDate { get; set; }
        public string DeliverTime { get; set; }
        public string DeliverNote { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
