using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class SpecialNumberView : BaseView
    {
        [Display(Name = "از شماره")]
        public int FromNumber { get; set; }

        [Display(Name = "تا شماره")]
        public int ToNumber { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
    }
}
