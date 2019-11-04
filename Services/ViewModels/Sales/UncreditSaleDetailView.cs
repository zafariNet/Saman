#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.Store;
#endregion

namespace Services.ViewModels.Sales
{
    public class UncreditSaleDetailView : BaseSaleDetailView
    {
        public Guid MainUncreditSaleDetailID { get; set; }

        [Display(Name = "مبلغ برگشت از فروش")]
        public long? MainUncreditSaleDetailLineTotal { get; set; }

        public Guid UncreditServiceID { get; set; }

        [Display(Name = "خدمات غیراعتباری")]
        public string UncreditServiceName { get; set; }

        //IEnumerables

        [Display(Name = "زیر فروشهای غیر اعتباری")]
        public virtual IEnumerable<UncreditServiceDeliveryView> UncreditServiceDeliveryViews { get; set; }

        // برگشت از فروشهای مربوط به این آیتم
        //public virtual IEnumerable<UncreditSaleDetailView> RollbackedUncreditSaleDetailViews { get; set; }
    }
}
