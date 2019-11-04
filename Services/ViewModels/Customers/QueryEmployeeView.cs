using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class QueryEmployeeView : BaseView
    {
        public Guid QueryID { get; set; }

        [Display(Name = "نما")]
        public string QueryTitle { get; set; }

        public Guid EmployeeID { get; set; }

        [Display(Name = "کارمند")]
        public string EmployeeName { get; set; }

    }
}
