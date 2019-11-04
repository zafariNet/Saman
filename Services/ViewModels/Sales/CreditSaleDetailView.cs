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
    public class CreditSaleDetailView : BaseSaleDetailView
    {
        public Guid MainCreditSaleDetailID { get; set; }

        [Display(Name = "مبلغ برگشت از فروش")]
        public long MainCreditSaleDetailLineTotal { get; set; }

        public Guid CreditServiceID { get; set; }

        [Display(Name = "خدمات اعتباری")]
        public string CreditServiceName { get; set; }

        public long PurchaseUnitPrice { get; set; }
        /// <summary>
        /// مبلغ برگشتی به شبکه
        /// </summary>
        public long RollbackNetworkPrice { get; set; }

        public string CustomerADSLPhone { get; set; }

        public string CustomerName { get; set; }

        //IEnumerables

        [Display(Name = "تحویل خدمات اعتباری")]
        public IEnumerable<CreditServiceDeliveryView> CreditServiceDeliveryViews { get; set; }

        // برگشت از فروشهای مربوط به این آیتم
        //public virtual IEnumerable<CreditSaleDetailView> RollbackedCreditSaleDetailViews { get; set; }
    }
}
