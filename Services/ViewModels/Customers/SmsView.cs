using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class SmsView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        [Display(Name = "فرستاده شد")]
        public bool Sent { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
    }
}
