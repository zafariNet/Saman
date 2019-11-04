using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.ReportCatalogService
{
    public class GetBonusComissionReportRequest
    {
        /// <summary>
        /// کاربران فعال یا غیر فعال
        /// </summary>
        public int ActiveUsers { get; set; }

        /// <summary>
        /// گروه یا گروه ها
        /// </summary>
        public IEnumerable<Guid?> GroupIDs { get; set; }

        /// <summary>
        /// کارمندان
        /// </summary>
        public IEnumerable<Guid?> EmployeeIDs { get; set; }

        /// <summary>
        /// تاریخ تایید پیک
        /// </summary>
        public string CourierConfirmedDate { get; set; }

        /// <summary>
        /// لیست کالاها
        /// </summary>
        public IEnumerable<Guid?> ProductIDs { get; set; }

        /// <summary>
        /// لیست محصولات
        /// </summary>
        public IEnumerable<Guid?> ProductPriceIDs { get; set; }


        /// <summary>
        /// شبکه
        /// </summary>
        public IEnumerable<Guid?> NetworkIDs { get; set; }

        /// <summary>
        /// خدمات اعتباری
        /// </summary>
        public IEnumerable<Guid?> CreditServiceIDs { get; set; }

        /// <summary>
        /// خدمات غیر اعتباری
        /// </summary>
        public IEnumerable<Guid?> UncreditServiceIDs { get; set; }

        /// <summary>
        ///وضعیت پیک 
        /// </summary>
        public int HasCourier { get; set; }

        /// <summary>
        /// محصول است ؟
        /// </summary>
        public bool IsProducts { get; set; }

        /// <summary>
        /// خدمات اعتباریست؟
        /// </summary>
        public bool IsCreditService { get; set; }

        /// <summary>
        /// خدمات غیر اعتباریست
        /// </summary>
        public bool IsUnCreditService { get; set; }

        /// <summary>
        /// تاریخ تایید پیک شروع
        /// </summary>
        public string CourierConfirmedStartDate { get; set; }

        /// <summary>
        /// تاریخ تایید پیک پایان
        /// </summary>
        public string CourierConfirmedEndDate { get; set; }

        /// <summary>
        /// تاریخ فروش شروع
        /// </summary>
        public string SaleStartDate { get; set; }

        /// <summary>
        /// تاریخ فروش پایان
        /// </summary>
        public string SaleEndDate { get; set; }

    }
}
