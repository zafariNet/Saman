using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class CodeView : BaseView
    {
        [Display(Name = "پیش شماره")]
        public string CodeName { get; set; }

        public Guid CenterID { get; set; }

        [Display(Name = "نام مرکز")]
        public string CenterName { get; set; }
    }
}
