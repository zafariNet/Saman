using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.ViewModels.Reports
{
    public class SupportHistoryView
    {
        /// <summary>
        /// تاریخ ورود
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public string StatusName { get; set; }
        
        /// <summary>
        /// متن خروجی
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Comment { get; set; }

        public string EmployeeName { get; set; }
    }
}
