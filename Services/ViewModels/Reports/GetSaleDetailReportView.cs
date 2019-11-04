using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class GetSaleDetailReportView
    {
        public Guid CustomerID { get; set; }
        /// <summary>
        /// نام مشتری
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// شماره تلفن مشتری
        /// </summary>
        [DisplayName("شماره تماس مشتری")]
        [Display(Name="تلفن مشتری")]
        
        public string ADSLPhone { get; set; }

        /// <summary>
        /// کارمند فروش
        /// </summary>
        public string SaleEmployeeName { get; set; }

        /// <summary>
        /// مرکز مخابراتی مشتری
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// تاریخ فروش
        /// </summary>
        public string SaleDate { get; set; }

        /// <summary>
        /// محصولات
        /// </summary>
        public string ProductPriceName { get; set; }

        /// <summary>
        /// کالاهای پایه
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// شبکه
        /// </summary>
        public string NetworkName { get; set; }

        /// <summary>
        /// نام خدمات اعتباری
        /// </summary>
        public string CreditServiceName { get; set; }

        /// <summary>
        /// نام خدمات غیر اعتباری
        /// </summary>
        public string UncreditServiceName { get; set; }

        /// <summary>
        /// قیمت کالا و خدمات
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// تخفیف
        /// </summary>
        public long Discount { get; set; }

        /// <summary>
        /// مالیات
        /// </summary>
        public long Imposition { get; set; }

        /// <summary>
        /// مجموع
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// کارمند تحویل
        /// </summary>
        public string DeliverEmployeeName { get; set; }

        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public string DeliverDate { get; set; }

        /// <summary>
        /// کارمند برگشت از فروش
        /// </summary>
        public string RollBackEmployeeName { get; set; }

        /// <summary>
        /// تاریخ برگشت از فروش
        /// </summary>
        public string RoolBackDate { get; set; }

        /// <summary>
        /// تعداد برگشت از فروش
        /// </summary>
        public int TotalRollBack { get; set; }

        /// <summary>
        /// مبلغ برگشت از فروش
        /// </summary>
        public long RollBackPrice { get; set; }

        /// <summary>
        /// نوع فاکتور
        /// </summary>
        public string SaleType { get; set; }

        /// <summary>
        /// امتیاز
        /// </summary>
        public long Bonus { get; set; }

        /// <summary>
        /// پورسانت
        /// </summary>
        public long Comission { get; set; }
        public string BonusDate { get; set; }
        public string ComissionDate { get; set; }

    }
}
