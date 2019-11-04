using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class AddCourierRequest
    {
        /// <summary>
        /// شناسه فاکور فروش
        /// </summary>
        public Guid SaleID { get; set; }

        /// <summary>
        /// تاریخ تحویل
        /// </summary>
        public string DeliverDate { get; set; }
        /// <summary>
        /// ساعت تحول
        /// </summary>
        public string DeliverTime { get; set; }

        /// <summary>
        /// مبلغ پیک
        /// </summary>
        public long CourierCost { get; set; }

        /// <summary>
        /// مبلغ دریافتی از مشتری
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// توضیحاتکاشناس فروش
        /// </summary>
        public string SaleComment { get; set; }

        /// <summary>
        /// تعداد واحد ها
        /// </summary>
        public int BuildingUnits { get; set; }

        /// <summary>
        /// نوع
        /// </summary>
        public int CourierType { get; set; }
    }
}
