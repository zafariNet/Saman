using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class SuctionModeView : BaseView
    {
        [Display(Name = "شیوه جذب")]
        public string SuctionModeName { get; set; }

        //IEnumerables

        //[Display(Name = "مشتریان")]
        //public IEnumerable<CustomerView> Customers { protected get;  set; }

        //// تعداد مشتریان این شیوه جذب
        //[Display(Name = "تعداد مشتریان این شیوه")]
        //public int CustomerCount
        //{
        //    get

        //    {
        //        int result = 0;
        //        if (Customers != null)
        //            result = Customers.Count();

        //        return result;
        //    }
        //}

    }
}
