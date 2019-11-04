using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.ReportCatalogService
{
    public class GetSuctionModeRequest
    {
        /// <summary>
        /// روش های جذب
        /// </summary>
        public IEnumerable<Guid?> SuctionModeIDs { get; set; }
        /// <summary>
        /// شیوه های جذب
        /// </summary>
        public IEnumerable<Guid?> SuctionModeDetailIDs { get; set; }

        /// <summary>
        /// شبکه ها
        /// </summary>
        public IEnumerable<Guid?> NetworkIDs { get; set; }

        /// <summary>
        /// شبکه ها
        /// </summary>
        public IEnumerable<Guid?> CenterIDs { get; set; }

        /// <summary>
        /// مرد / زن
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// نمایندگان
        /// </summary>
        public IEnumerable<Guid?> AgencyIDs { get; set; }
        /// <summary>
        /// گروه ها
        /// </summary>
        public IEnumerable<Guid?> GroupIDs { get; set; }
        /// <summary>
        /// کارمندان
        /// </summary>
        public IEnumerable<Guid?> EmployeeIds { get; set; }
        /// <summary>
        /// تاریخ شروع ثبت نام
        /// </summary>
        public string RegisterStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان ثبت نام
        /// </summary>
        public string RegisterEndDate { get; set; }
        /// <summary>
        /// دارای صندوق
        /// </summary>
        public bool HasFiscal { get; set; }
        /// <summary>
        /// فقط کاربران فعال
        /// </summary>
        public int ActiveUsers { get; set; }
    }
}
