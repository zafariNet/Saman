using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.ViewModels.StoreCatalog
{
    public class ProductPriceForCreate
    {
        public Guid ProductPriceID { get; set; }

        public int Units { get; set; }

        public long Discount { get; set; }
    }
}
