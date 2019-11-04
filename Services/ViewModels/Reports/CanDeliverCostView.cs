using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class CanDeliverCostView
    {
        public string Domain { get; set; }
        public string Month { get; set; }
        public string Type { get; set; }
        public string SaleType { get; set; }
        public string SaleDetailType { get; set; }
        public long Bed { get; set; }
        public long Bes { get; set; }

    }

    public enum Month
    {
        فروردین = 1,
        اردیبهشت = 2,
        خرداد = 3,
        تیر = 4,
        مرداد = 5,
        شهریور = 6,
        مهر = 7,
        آبان = 8,
        آذر = 9,
        دی = 10,
        بهن = 11,
        اسفند = 12
    }
}
