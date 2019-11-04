using System;

namespace Controllers.ViewModels.Reports
{
    public class SaleDetailReportView
    {
        public int Units { get; set; }

        public long Discount { get; set; }

        public string SaleDetail { get; set; }

        public long Imposition { get; set; }

        public long UnitPrice { get; set; }

        public long LineTotalWithoutDiscountAndImposition { get; set; }

    }
}
