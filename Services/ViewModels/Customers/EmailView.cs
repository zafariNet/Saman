using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class EmailView : BaseView
    {
        public Guid CustomerID { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        [Display(Name = "فرستاده شده")]
        public bool Sent { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
        }
}
