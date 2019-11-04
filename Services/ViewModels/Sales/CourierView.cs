using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Sales
{
    public class CourierView : BaseView
    {
        /// <summary>
        /// نام مشتری
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// شناسه مشتری
        /// </summary>
        public Guid CustomerID { get; set; }

        /// <summary>
        /// تلفن متقاضی
        /// </summary>
        public string ADSLPhone { get; set; }

        /// <summary>
        /// آدرس متقاضی
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// شناسه پیک
        /// </summary>
        public Guid CourierEmployeeID { get; set; }

        /// <summary>
        /// امتیاز
        /// </summary>
        public long Bonus { get; set; }

        /// <summary>
        /// کارشناس فروش
        /// </summary>
        public string SaleEmployeeName { get; set; }

        /// <summary>
        /// شاسه فاکتور
        /// </summary>
        public Guid SaleID { get; set; }

        /// <summary>
        ///تاریخ اعزام پیک
        /// </summary>
        public string DeliverDate { get; set; }

        /// <summary>
        /// ساعت اعزم پیک
        /// </summary>
        public string DeliverTime { get; set; }

        /// <summary>
        /// مبلغ پیک
        /// </summary>
        public long CourierCost { get; set; }

        /// <summary>
        /// مبلغ باقیمانده فاکتور
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// توضیحات کارشناس فروش
        /// </summary>
        public string SaleComment { get; set; }

        /// <summary>
        /// توضیحات کارشاس پیک
        /// </summary>
        public string ExpertComment { get; set; }

        /// <summary>
        /// تعداد واحد ها
        /// </summary>
        public int BuildingUnits { get; set; }

        /// <summary>
        /// نوع تحویل
        /// </summary>
        public  int CourierType { get; set; }

        /// <summary>
        /// وضعیت پیک
        /// </summary>
        public int CourierStatuse { get; set; }

        /// <summary>
        /// نام پیک
        /// </summary>
        public  string  CourierEmployeeName { get; set; }

        /// <summary>
        /// شیوه جذب
        /// </summary>
        public string SuctionModeName { get; set; }
    }
}
