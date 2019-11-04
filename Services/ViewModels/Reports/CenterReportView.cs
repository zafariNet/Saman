using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class CenterReportView
    {
        public Guid ID { get; set; }
        /// <summary>
        /// نام شبکه
        /// </summary>
        public string CenterkName { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public int Count { get; set; }
    }
}
