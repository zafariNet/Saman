using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Sales
{
    public class GeneralSaleDetailView:BaseView
    {
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public int Unit { get; set; }
        public long UnitPrice { get; set; }
        public long LineDiscount { get; set; }
        public long LineImposition { get; set; }
        public long LinetoalWithoutDiscountAndImposition { get; set; }
        public long LineTotal { get; set; }
        public string SaleDate { get; set; }
        public string ConfirmedDate { get; set; }
        public string ConfirmEployeeName { get; set; }
    }
}
