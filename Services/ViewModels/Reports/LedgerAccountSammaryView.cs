using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class LedgerAccountSammaryView
    {
        public Guid CustomerID { get; set; }
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// نام مشتری
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// شماره تلفن مشتری
        /// </summary>
        public string ADSLPhone { get; set; }

        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// نوع تراکنش==> فاکتور - فاکتور برگشت - مالی
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// مبلغ کل بدون مالیت و تخفیف
        /// </summary>
        public long TotalPriceWithoutDiscountAndImposition { get; set; }

        /// <summary>
        /// مجموع تخفیف ها
        /// </summary>
        public long TotalDiscount { get; set; }

        /// <summary>
        /// مجموع تخفیف ها
        /// </summary>
        public long TotalImposition { get; set; }

        /// <summary>
        /// شماره سند
        /// </summary>
        public string Documentserial { get; set; }

        /// <summary>
        /// مجموع کل
        /// </summary>
        public long TotalPrice { get; set; }

        /// <summary>
        /// مبلغ بدهکار
        /// </summary>
        public long BedCost { get; set; }

        /// <summary>
        /// مبلغ بستانکار
        /// </summary>
        public long BesCost { get; set; }

        /// <summary>
        /// باقیمانده
        /// </summary>
        public long Remain { get; set; }

        /// <summary>
        /// وضعیت فعلی
        /// </summary>
        public string Status { get; set; }

        public char RecordType { get; set; }

        public bool EditCourier { get; set; }

        public bool RollbackLightOn { get; set; }

        public bool CanDeliver { get; set; }

        public bool  CanRollback { get; set; }

    }
}
