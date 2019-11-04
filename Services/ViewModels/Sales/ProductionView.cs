using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Sales
{
    public class ProductionView
    {
        public Guid SaleDetailID { get; set; }
        
        //Added By Zafari
        public Guid ProductID { get; set; }

        public int ProductCode { get; set; }

        public string SaleDetailName { get; set; }

        public long UnitPrice { get; set; }

        public long MaxDiscount { get; set; }

        public long Imposition { get; set; }

        public string ProductType { get; set; }
        
        public string Note { get; set; }
    }
    
}
