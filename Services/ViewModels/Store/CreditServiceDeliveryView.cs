using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class CreditServiceDeliveryView : BaseView
    {
        public Guid CreditServiceID { get; set; }

        [Display(Name = "خدمات")]
        public string CreditServiceName { get; set; }

        public Guid CreditSaleDetailID { get; set; }

        [Display(Name = "جزئیات")]
        public string CreditSaleDetailLineTotal { get; set; }

        public Guid EmployeeID { get; set; }

        [Display(Name = "کارمند")]
        public string EmployeeName { get; set; }

        [Display(Name = "تعداد")]
        public int Units { get; set; }

        [Display(Name = "وارد/خارج")]
        public bool ExitedFromStore { get; set; }

        [Display(Name = "تاریخ")]
        public string ExitDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
    }
}
