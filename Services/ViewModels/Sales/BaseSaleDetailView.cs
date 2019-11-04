#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.Store;
using Model.Sales;
#endregion

namespace Services.ViewModels.Sales
{
    public class BaseSaleDetailView : BaseView
    {
        public Guid SaleID { get; set; }

        [Display(Name = "شماره فاکتور")]
        public string SaleNumber { get; set; }

        public string RollbackEmployeeName { get; set; }

        [Display(Name = "قیمت واحد")]
        public long UnitPrice { get; set; }

        [Display(Name = "تعداد")]
        public int Units { get; set; }

        [Display(Name = "تخفیف")]
        public long Discount { get; set; }

        public string RollbackDate { get; set; }

        [Display(Name = "تخفیف سطر")]
        public long LineDiscount { get; set; }

        [Display(Name = "مالیات")]
        public long Imposition { get; set; }

        [Display(Name = "مالیات سطر")]
        public long LineImposition { get; set; }

        [Display(Name = "جمع سطر")]
        public long LineTotal { get; set; }

        // جمع سطر بدون تخفیف و مالیات
        public long LineTotalWithoutDiscountAndImposition { get; set; }

        [Display(Name = "توضیحات برگشت")]
        public string RollbackNote { get; set; }
        
        //  آیا آیتم قابل تحویل است؟
        public bool CanDeliver { get; set; }
        /// <summary>
        /// مبلغ برگشتی
        /// </summary>
        public long RollbackPrice { get; set; }

        // آیا این آیتم برگشت خوده است؟
        public bool Rollbacked { get; set; }

        // آیا این آیتم مربوط به فاکتور برگشت از فروش است؟
        public bool IsRollbackDetail { get; set; }
       /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public string DeliverDate { get; set; }
        /// <summary>
        /// تحویل شده
        /// </summary>
        public bool Delivered { get; set; }
        /// <summary>
        /// توضیحات تحویل
        /// </summary>
        public string DeliverNote { get; set; }
        /// <summary>
        /// کارمند تحویل
        /// </summary>
        public string DeliverEmployeeName { get; set; }

        public SaleDetailStatus Status { get; set; }
    }
}
