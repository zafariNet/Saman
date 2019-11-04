using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
namespace Services.Messaging.SaleCatalogService
{
    public class SaleReportRequest
    {
        /// <summary>
        /// کارمند فروش
        /// </summary>
        public IEnumerable<Guid> SaleEmployeeID { get; set; }

        /// <summary>
        /// کارمند تحویل
        /// </summary>
        public IEnumerable<Guid> DeliverEmployeeID { get; set; }

        /// <summary>
        /// کارمند برگشت از فروش
        /// </summary>
        public IEnumerable<Guid> RollBackEmployeeID { get; set; }

        /// <summary>
        /// کالاهای پایه
        /// </summary>
        public IEnumerable<Guid> Products { get; set; }

        /// <summary>
        /// شبکه ها
        /// </summary>
        public IEnumerable<Guid> Networks { get; set; }

        /// <summary>
        /// لیست محصولات
        /// </summary>
        public IEnumerable<Guid> ProductPrices { get; set; }

        /// <summary>
        /// خدمات غیر اعتباری
        /// </summary>
        public IEnumerable<Guid> UncreditServices { get; set; }

        /// <summary>
        /// خدمات اعتباری
        /// </summary>
        public IEnumerable<Guid> CreditService { get; set; }

        /// <summary>
        /// تاریخ فروش از
        /// </summary>
        public string SaleStartDate { get; set; }

        /// <summary>
        /// تاریخ فروش تا
        /// </summary>
        public string SaleEndDate { get; set; }

        /// <summary>
        /// تاریخ فروش از
        /// </summary>
        public string RollBackStartDate { get; set; }

        /// <summary>
        /// تاریخ تحویل از
        /// </summary>
        public string DeliverStartDate { get; set; }

        /// <summary>
        /// تاریخ تحویل تا
        /// </summary>
        public string DeliverEndDate { get; set; }

        /// <summary>
        /// تاریخ فروش تا
        /// </summary>
        public string RollBackEndDate { get; set; }

        /// <summary>
        /// تایید شده ها یا نشده ها
        /// </summary>
        public bool? Deliverd { get; set; }

        /// <summary>
        /// تایید شده یا نشده
        /// </summary>
        public bool? Confirmed { get; set; }

        /// <summary>
        /// برگشت شده یا نشده
        /// </summary>
        public bool? RollBacked { get; set; }
    }
}
