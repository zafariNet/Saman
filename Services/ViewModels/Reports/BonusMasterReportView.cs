using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class BonusMasterReportView
    {
     
        /// <summary>
        /// کارشناس فروش
        /// </summary>
        public string SaleEmployeeName { get; set; }

        /// <summary>
        /// امتیاز کالا
        /// </summary>
        public long ProductBonus { get; set; }

        /// <summary>
        /// امتیاز خدمات اعتباری
        /// </summary>
        public long CreditServiceBonus { get; set; }

        /// <summary>
        /// امتیاز خدمات غیر اعتباری
        /// </summary>
        public long UncreditServiceBonus { get; set; }

    }

    public class ComissionMasterReportView
    {

        /// <summary>
        /// کارشناس فروش
        /// </summary>
        public string SaleEmployeeName { get; set; }

        /// <summary>
        /// امتیاز کالا
        /// </summary>
        public long ProductComission { get; set; }

        /// <summary>
        /// امتیاز خدمات اعتباری
        /// </summary>
        public long CreditServiceComission { get; set; }

        /// <summary>
        /// امتیاز خدمات غیر اعتباری
        /// </summary>
        public long UncreditServiceComission { get; set; }

        public string DeliverDate { get; set; }
        public string ComissionDate { get; set; }
        public string CreateEmployeeName { get; set; }

    }
}
