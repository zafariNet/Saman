using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class StoreProductView : BaseView
    {
        public Guid ProductID { get; set; }

        [Display(Name = "نام کالا")]
        public string ProductName { get; set; }

        [Display(Name = "موجودی انبار")]
        public int ProductsUnistInStock { get; set; }

        public Guid StoreID { get; set; }

        [Display(Name = "انبار")]
        public string StoreName { get; set; }

        [Display(Name = "تعداد")]
        public int UnitsInStock { get; set; }

        public string OwnerEmployeeName { get; set; }
    }
}
