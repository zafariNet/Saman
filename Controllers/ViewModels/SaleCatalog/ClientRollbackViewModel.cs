using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Controllers.ViewModels.SaleCatalog
{
    public class ClientRollbackViewModel
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
        /// توضیحات برگشت از فروش
        /// </summary>
        public string RollbackNote { get; set; }
        /// <summary>
        /// مبلغ برگشت از فروش
        /// </summary>
        public long RollbackPrice { get; set; }

        /// <summary>
        /// مبلغی که به شبکه برمیگردد
        /// </summary>
        public long? RollbackNetworkPrice { get; set; }
        /// <summary>
        /// تاریخ برگشت از فروش
        /// </summary>
        public string RollbackDate { get; set; }
        /// <summary>
        /// کارمند برگشت از فروش
        /// </summary>
        public string RollBackEmployeeName { get; set; }

        /// <summary>
        /// مبلغ مالیات قابل برگشت
        /// </summary>
        public long CanRollbackImpositionPrice { get; set; }

        /// <summary>
        /// مبلغ تخفیف قابل برگشت 
        /// </summary>
        public long CanRollbackDiscountPrice { get; set; }

        public bool RoleBacked { get; set; }

        public bool IsDeliverdBefor { get; set; }

        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public string DeliverDate { get; set; }


    }
}
