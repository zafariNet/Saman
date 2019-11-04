using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class SuctionModeDetailview:BaseView
    {
        public string SuctionModeDetailname { get; set; }

        public string SuctionMode { get; set; }

        public string Discontinued { get; set; }

        public Guid ParentID { get; set; }

        //[Display(Name = "مشتریان")]
        //public IEnumerable<CustomerView> Customers { protected get; set; }

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
