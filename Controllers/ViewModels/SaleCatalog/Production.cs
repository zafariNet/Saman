using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Controllers.ViewModels.SaleCatalog
{
    public class Production : BasePageView
    {
        [ScaffoldColumn(false)]
        public Guid SaleDetailID { get; set; }
        
        //Added By Zafari
        [DisplayName("کد کالا")]
        public Guid ProductID { get; set; }

        public int ProductCode { get; set; }

        [DisplayName("شرح")]
        public string SaleDetailName { get; set; }

        [DisplayName("قیمت واحد")]
        public long UnitPrice { get; set; }

        [DisplayName("ماکزیمم تخفیف")]
        public long MaxDiscount { get; set; }

        [DisplayName("مالیات واحد")]
        public long Imposition { get; set; }

        public string ProductType { get; set; }
        
        [DisplayName("شرح محصول")]
        public string Note { get; set; }
    }
}