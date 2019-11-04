using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SaleCatalogService
{
    public class DeliverRequest
    {
        /// <summary>
        /// RowID
        /// </summary>
        public Guid SaleDetailID { get; set; }
        /// <summary>
        /// نوع آیتم فروش
        /// </summary>
        public char DetailType { get; set; }

        public Guid DeliverEmployeeID { get; set; }

        public string DeliverNote { get; set; }
    }
}
