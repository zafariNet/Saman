using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class FollowStatusView : BaseView
    {
        [Display(Name = "وضعیت پیگیری")]
        public string FollowStatusName { get; set; }

        //IEnumerables

        //[Display(Name = "مشتریان")]
        //public IEnumerable<CustomerView> Customers { protected get; set; }

        //// تعداد مشتریان این وضعیت
        //public int CustomerCount
        //{
        //    get
        //    {
        //        return Customers.Count();
        //    }
        //}

    }
}
