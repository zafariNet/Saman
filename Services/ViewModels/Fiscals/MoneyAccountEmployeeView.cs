using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Fiscals
{
    public class MoneyAccountEmployeeView : BaseView
    {
        public Guid EmployeeID { get; set; }

        [Display(Name = "کارمند تأیید کننده")]
        public string EmployeeName { get; set; }

        public Guid MoneyAccountID { get; set; }

        [Display(Name = "حساب مالی جهت تأیید")]
        public string MoneyAccountName { get; set; }
    }
}
