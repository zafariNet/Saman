using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Customers
{
    public class NetworkCenterPriorityView:BaseView
    {
        public Guid NetworkID { get; set; }
        public string NetworkName { get; set; }
        public Guid CenterID { get; set; }
        public string CenterName { get; set; }
        public int SalePriority { get; set; }
    }
}
