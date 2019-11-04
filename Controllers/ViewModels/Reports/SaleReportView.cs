#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Controllers.ViewModels.Reports
{
    public class SaleReportView
    {
        public Guid CustomerID { get; set; }
        public Guid SaleID { get; set; }
        /// <summary>
        /// شماره فاکتور
        /// </summary>
        public string SaleNumber { get; set; }
        /// <summary>
        /// نام مشتری
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// مرکز مخابراتی
        /// </summary>
        public string CenterName { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// همراه
        /// </summary>
        public string Mobile1 { get; set; }
        /// <summary>
        /// تاریخ
        /// </summary>
        public string Date { get; set; }
      /// <summary>
        /// تلفن ADSL‏
        /// </summary>
        public string ADSLPhone { get; set; }
        /// <summary>
        /// نام صاحب خط
        /// </summary>
        public string CustomerSName { get; set; }
        /// <summary>
        /// اقلام فاکتور
        /// </summary>
        public IEnumerable<SaleDetailReportView> SaleDetails { get; set; }
        /// <summary>
        /// جمع کل فاکتور بدون تخفیف و مالیات
        /// </summary>
        public long SaleTotalWithoutDisAndImp { get; set; }
        /// <summary>
        /// جمع کل تخفیف
        /// </summary>
        public long AllDiscount { get; set; }
        /// <summary>
        /// جمع کل مالیات
        /// </summary>
        public long AllImposition { get; set; }
        /// <summary>
        /// مبلغ قابل پرداخت
        /// </summary>
        public long SaleTotal { get; set; }
        /// <summary>
        /// نام کارمند فروش
        /// </summary>
        public string SaleEmployeeName { get; set; }

    }
}
