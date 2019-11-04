using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Controllers.ViewModels.SaleCatalog
{
    public class ClientDeliverViewModel
    {
        /// <summary>
        /// آیدی آیتم فروش
        /// </summary>
        public Guid SaleDetailID { get; set; }
        /// <summary>
        /// شرح
        /// </summary>
        public string SaleDetailName { get; set; }
        /// <summary>
        /// آیدی مربوط به سطر
        /// </summary>
        public Guid RowID { get; set; }
        /// <summary>
        /// تعداد
        /// </summary>
        public int Units { get; set; }
        /// <summary>
        /// توضیحات تحویل
        /// </summary>
        public string DeliverNote { get; set; }
        /// <summary>
        /// کارمند تحویل
        /// </summary>
        public string DeliverEmployeeName { get; set; }
        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public string DeliverDate { get; set; }
        /// <summary>
        /// قابلیت تحویل وجود دارد؟
        /// </summary>
        public bool CanDeliver { get; set; }

        /// <summary>
        /// جمع ردیف بدون مالیات و تخفیف
        /// </summary>
        public long LineTotalWithoutDiscountAndImposition { get; set; }

        /// <summary>
        /// مجموع تخفیف ها
        /// </summary>
        public long LineDiscount { get; set; }

        /// <summary>
        /// مجموع مالیات ها
        /// </summary>
        public long LineImposition { get; set; }

        public long LineTotal { get; set; }
    }
}
