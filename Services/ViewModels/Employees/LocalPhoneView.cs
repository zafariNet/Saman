using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Model.Employees;

namespace Services.ViewModels.Employees
{
    public class LocalPhoneView : BaseView
    {
        [Display(Name = "شماره داخلی")]
        public string LocalPhoneNumber { get; set; }

        public string OwnerEmployeeName { get; set; }
    }
}
