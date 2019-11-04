using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class NetworkView : BaseView
    {
        [Display(Name = "نام شبکه")]
        public string NetworkName { get; set; }
        
        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "تحویل هنگام کمبود اعتبار")]
        public bool DeliverWhenCreditLow { get; set; }

        [Display(Name = "موجودی")]
        public long Balance { get; set; }

        [Display(Name = "فعال بودن")]
        public bool Discontinued { get; set; }

        public string Alias { get; set; }

        public int SortOrder { get; set; }
        //IEnumerables
        
        [Display(Name = "مشتریان")]
        public IEnumerable<Customers.CustomerView> Customers { get; protected set; }
        
        [Display(Name = "شبکه مرکز ها")]
        public IEnumerable<Customers.NetworkCenterView> NetworkCenters { get; protected set; }
        
        [Display(Name = "خدمات اعتباری")]
        public IEnumerable<CreditServiceView> CreditServices { get; protected set; }

        public bool CanSale { get; set; }
    }

    public class NetworkSummaryView : BaseView
    {
        [Display(Name = "نام شبکه")]
        public string NetworkName { get; set; }
        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "تحویل هنگام کمبود اعتبار")]
        public bool DeliverWhenCreditLow { get; set; }

        [Display(Name = "موجودی")]
        public long Balance { get; set; }

        [Display(Name = "فعال بودن")]
        public bool Discontinued { get; set; }

        public string Alias { get; set; }

        public int SortOrder { get; set; }
    }
}
