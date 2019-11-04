using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.ReportCatalogService
{
    public class GetNetworkReportView
    {
        public Guid ID { get; set; }
        /// <summary>
        /// نام شبکه
        /// </summary>
        public string NetworkName { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public int Count { get; set; }
    }
}
