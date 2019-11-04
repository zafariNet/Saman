using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class BuyPossibilityView : BaseView
    {
        [Display(Name = "احتمال خرید")]
        public string BuyPossibilityName { get; set; }

        //IEnumerables

        //[Display(Name = "مشتریان")]
        //public IEnumerable<CustomerView> Customers { protected get; set; }

        //public int CustomerCount
        //{
        //    get
        //    {
        //        return Customers.Count();
        //    }
        //}

    }
}
