using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class ProductPriceView : BaseView
    {
        public Guid ProductID { get; set; }

        [Display(Name = "نام کالا")]
        public string ProductName { get; set; }

        [Display(Name = "شرح")]
        public string ProductPriceTitle { get; set; }

        [Display(Name = "کد محصول")]
        public int ProductPriceCode { get; set; }

        [Display(Name = "قیمت واحد")]
        public long UnitPrice { get; set; }

        [Display(Name = "ماکزیمم تخفیف")]
        public long MaxDiscount { get; set; }

        [Display(Name = "مالیات")]
        public long Imposition { get; set; }

        [Display(Name = "غیر فعال")]
        public bool Discontinued { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "ترتیب")]
        public int? SortOrder { get; set; }

        public long Comission { get; set; }

        public long Bonus { get; set; }
    }
}
