using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class UncreditServiceView : BaseView
    {
        [Display(Name = "خدمت غیر اعتباری")]
        public string UncreditServiceName { get; set; }

        [Display(Name = "کد خدمت غیر اعتباری")]
        public int UnCreditServiceCode { get; set; }

        [Display(Name = "قیمت واحد")]
        public int UnitPrice { get; set; }

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

        //IEnumerables

        //[Display(Name = "فروشهای غیر اعتباری")]
        //public IEnumerable<Sales.UncreditSaleDetailView> UncreditSaleDetails { get; protected set; }
        
        //[Display(Name = "تحویل خدمات غیر اعتباری")]
        //public IEnumerable<UncreditServiceDeliveryView> UncreditServiceDeliverys { get; protected set; }
    }
}
