using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.Store;

namespace Services.ViewModels.Sales
{
    public class ProductSaleDetailView : BaseSaleDetailView
    {
        public Guid MainProductSaleDetailID { get; set; }

        [Display(Name = "مبلغ برگشت از فروش")]
        public long MainProductSaleDetailLineTotal { get; set; }

        public Guid ProductID { get; set; }

        [Display(Name = "کالا")]
        public string ProductPriceTitle { get; set; }

        [Display(Name = "کالا")]
        public Guid ProductPriceID { get; set; }

        public Guid DeliverStoreID { get; set; }

        public string DeliverStoreName { get; set; }

        //IEnumerables

        //[Display(Name = "تحویل کالاها")]
        //public virtual IEnumerable<ProductDeliveryView> ProductDeliveryViews { get; set; }

        // برگشت از فروشهای مربوط به این آیتم
        //public virtual IEnumerable<ProductSaleDetailView> RollbackedProductSaleDetailViews { get; set; }
    }
}
