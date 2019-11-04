using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Support
{
    public class SupportOwnView:BaseView
    {
        public Guid SupportID { get; set; }

        public Guid CustomerID { get; set; }

        public string ExpertEmployeeName { get; set; }

        public string CustomerName { get; set; }

        public string ADSLPhone { get; set; }

        public string CenterName { get; set; }

        public string Address { get; set; }

        public string DispatchDate { get; set; }

        public string DispatchTime { get; set; }

        public long Balance { get; set; }

        public string Type { get; set; }

        public string NetworkName { get; set; }

        public bool HasNotDeliveredProducts { get; set; }

        public string Comment { get; set; }
    }
}
