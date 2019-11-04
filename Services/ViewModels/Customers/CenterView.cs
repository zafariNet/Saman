using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class CenterView : BaseView
    {
        [Display(Name = "نام مرکز")]
        public string CenterName { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "تعداد مشتریان")]
        public string CustomerCount { get; set; }

        [Display(Name = "وضعیت")]
        public string Status { get; set; }

        [Display(Name = "وضعیت")]
        // dar Ext hazf shavad.
        public string StatusFa
        {
            get
            {
                //switch(Status){
                //    case "AdameEmkan":
                //        return "عدم امکان موقت";
                //    case "تحت پوشش  ":
                //        return "تحت پوشش";
                //    default:
                //        return "عدم پوشش";
                return Status;
            
            }
        }


        //IEnumerables

        [Display(Name = "پیش شماره ها")]
        public IEnumerable<CodeView> Codes { get; protected set; }

        [Display(Name = "مشتریان")]
        public IEnumerable<CustomerView> Customers { get; protected set; }

        [Display(Name = "وضعیت شبکه ها در این مرکز")]
        public IEnumerable<NetworkCenterView> NetworkCenters { get; set; }
    }
}
