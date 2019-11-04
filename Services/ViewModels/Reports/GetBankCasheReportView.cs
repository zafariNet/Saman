using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Reports
{
    public class GetBankCasheReportView
    {
        /// <summary>
        /// نام مشتری
        /// </summary>
        public string Name { get; set; }
        public string ADSLPhone { get; set; }
        public string CreateEmployeeName { get; set; }
        public string ConfirmEmployeeName { get; set; }
        public string InvestDate { get; set; }
        public string ConfirmDate { get; set; }
        public long Cost { get; set; }
        public long ConfiremdCost { get; set; }
        public long SerialNumber { get; set; }
        public string Bed { get; set; }
        public string Bes { get; set; }
        public string MoneyAccountName { get; set; }
        public string DocType { get; set; }
        public string Note { get; set; }
        public string AccountingSerialNumber { get; set; }
        public long FiscalReciptNumber { get; set; }
    }
}
