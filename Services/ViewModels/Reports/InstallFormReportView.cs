using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class InstallFormReportView
    {
        /// <summary>
        /// تاریخ نصب
        /// </summary>
        public string InstallDate { get; set; }

        /// <summary>
        /// ساعت نصب
        /// </summary>
        public string InstallTime { get; set; }

        /// <summary>
        /// کالای تحویل نشده
        /// </summary>
        public bool HasUnDeliveredProducts { get; set; }

        /// <summary>
        /// باقیمانده
        /// </summary>
        public long? Balance { get; set; }

        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public string InstallerName { get; set; }

        /// <summary>
        /// نام مشتری
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// تلفن مشتری
        /// </summary>
        public string ADSLPhone { get; set; }

        /// <summary>
        /// آدس مشتری
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// نام شبکه
        /// </summary>
        public string NetworkName { get; set; }

        /// <summary>
        /// توضیحات شبکه
        /// </summary>
        public string NetworkNote { get; set; }

        /// <summary>
        /// نام نمایشی شبکه
        /// </summary>
        public string Alias { get; set; }
    }
}
