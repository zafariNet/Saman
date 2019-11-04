using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class ProductCategoryView : BaseView
    {
        [Display(Name = "طبقه")]
        public string ProductCategoryName { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "غیرفعال")]
        public bool Discontinued { get; set; }

        //IEnumerables
        
        [Display(Name = "کالاها")]
        public IEnumerable<ProductView> Products { get; protected set; }
    }
}
