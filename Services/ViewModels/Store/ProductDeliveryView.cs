using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class ProductDeliveryView : BaseView
    {
        public Guid ProductID { get; set; }

        [Display(Name = "کالا")]
        public string ProductName { get; set; }

        public Guid ProductSaleDetailID { get; set; }

        [Display(Name = "جزئیات")]
        public string ProductSaleDetailLineTotal { get; set; }

        public Guid ExitEmployeeID { get; set; }

        [Display(Name = "کارمند")]
        public string ExitEmployeeName { get; set; }

        [Display(Name = "تعداد")]
        public int Units { get; set; }

        [Display(Name = "خروج/ورود")]
        public bool ExitedFromStore { get; set; }

        [Display(Name = "تاریخ")]
        public string ExitDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
    }
}
