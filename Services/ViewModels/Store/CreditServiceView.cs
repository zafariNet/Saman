using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class CreditServiceView : BaseView
    {
        public Guid NetworkID { get; set; }

        [Display(Name = "شبکه")]
        public string NetworkName { get; set; }

        [Display(Name = "خدمت اعتباری")]
        public string ServiceName { get; set; }

        [Display(Name = "کد خدمات اعتباری")]
        public int CreditServiceCode { get; set; }

        [Display(Name = "قیمت واحد")]
        public long UnitPrice { get; set; }

        [Display(Name = "قیمت خرید")]
        public string PurchaseUnitPrice { get; set; }

        [Display(Name = "قیمت فروش به نمایندگان")]
        public string ResellerUnitPrice { get; set; }

        [Display(Name = "ماکزیمم مقدار تخفیف")]
        public long MaxDiscount { get; set; }

        [Display(Name = "مالیات")]
        public int Imposition { get; set; }

        [Display(Name = "غیر فعال")]
        public bool Discontinued { get; set; }

        [Display(Name = "روزهای انقضا")]
        public int ExpDays { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "ترتیب")]
        public int SortOrder { get; set; }

        public long Comission { get; set; }

        public long Bonus { get; set; }

        //IEnumerables
        
        //[Display(Name = "فروش اعتباری")]
        //public IEnumerable<Sales.CreditSaleDetailView> CreditSaleDetails { get; protected set; }
        
        //[Display(Name = "تحویل خدمات اعتباری")]
        //public IEnumerable<CreditServiceDeliveryView> CreditServiceDeliverys { get; protected set; }
    }
}
