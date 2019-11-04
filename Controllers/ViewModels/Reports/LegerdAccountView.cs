using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Services.ViewModels.Customers;
using Services.ViewModels.Fiscals;
using Services.ViewModels.Sales;

namespace Controllers.ViewModels.FiscalCatalog
{
    public class LedgerAccountView 
    {
        public Guid CustomerID { get; set; }
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// شناسه فروش
        /// </summary>
        public Guid SaleID { get; set; }
        /// <summary>
        /// مبلغ بدون مالیات و تخفیف
        /// </summary>
        public long LineTotalWithoutDiscountAndImposition { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public int Units { get; set; }

        /// <summary>
        /// تخفیف
        /// </summary>
        public long Discount { get; set; }

        /// <summary>
        /// مالیات
        /// </summary>
        public long Imposition { get; set; }
        // تاریخ
        public string Date { get; set; }
        // شرح
        public string Description { get; set; }
        // شماره صورتحساب یا فیش
        public string SerialNumber { get; set; }
        // مبلغ بد
        public long BedCost { get; set; }
        // مبلغ بس
        public long BesCost { get; set; }
        // دریافت - پرداخت
        public string FiscalType { get; set; }
        // مانده
        public long Remain { get; set; }
        // وضعیت تحویل
        public string DeliverStatus { get; set; }
        // وضعیت برگشت
        public string RollbackStatus { get; set; }

        public bool CanDeliver { get; set; }

        public bool CanRollback { get; set; }

        public char RecordType { get; set; }

        /// <summary>
        /// جهت تشخیص اینکه از کدام نوع خدمات است.
        /// </summary>
        public string Type { get; set; }
        
     }
}
