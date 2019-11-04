using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Employees
{
    public class PermissionView : BaseView
    {
        [Display(Name = "شناسه کارمند")]
        public Guid EmployeeID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "گروه")]
        public string Group { get; set; }

        [Display(Name = "کلید")]
        public string Key { get; set; }

        [Display(Name = "مقدار")]
        public bool Guaranteed { get; set; }
    }
}
