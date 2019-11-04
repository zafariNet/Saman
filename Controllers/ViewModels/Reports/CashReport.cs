using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.ViewModels.Reports
{
    public class CashReport:BasePageView
    {
        public string CashDate { get; set; }
        public string TransactionType { get; set; }
        public long DisplayCost { get; set; }
        public string DocumentType { get; set; }
        public long SerialNumber { get; set; }
        public string ConfirmDate { get; set; }
        /// <summary>
        /// تاریخ واریز
        /// </summary>
        public string InvestDate { get; set; }
        public string MoneyAccountName { get; set; }
        /// <summary>
        /// کاربر ایجاد کننده
        ///  </summary>
        public string CreateEmployeeName { get; set; }
        /// <summary>
        /// نام مشتری
        /// </summary>
        public string CustomerName { get; set; }
        public string Mobile1 { get; set; }
        public string ADSLPhone { get; set; }
        public string Address { get; set; }
        public long ConfiremdCost { get; set; }
        public string ConfirmEmpoloyeeName { get; set; }
        public string Note { get; set; }
        public long FiscalReciptNumber { get; set; }
    }
}
