using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddBonusComissionRequest
    {

        /// <summary>
        /// فروش کالا
        /// </summary>
        public Guid? ProductSaleDetailID { get; set; }

        /// <summary>
        /// خدمات غیر اعتباری
        /// </summary>
        public Guid? UncreditSaleDetailID { get; set; }

        /// <summary>
        /// خدمات اعتباری
        /// </summary>
        public Guid? CreditSaleDetailID { get; set; }

        /// <summary>
        /// پیک
        /// </summary>
        public Guid? CourierID { get; set; }

        /// <summary>
        /// مشتری
        /// </summary>
        public Guid CustomerID { get; set; }


        /// <summary>
        /// پورسانت
        /// </summary>
        public int Comission { get; set; }

        public int Bonus { get; set; }

    }
}
