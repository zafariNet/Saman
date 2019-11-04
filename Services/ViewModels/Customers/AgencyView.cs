using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class AgencyView : BaseView
    {
        [Display(Name = "نام نمایندگی")]
        public string AgencyName { get; set; }

        [Display(Name = "نام مدیر")]
        public string ManagerName { get; set; }

        [Display(Name = "تلفن 1")]
        public string Phone1 { get; set; }

        [Display(Name = "تلفن 2")]
        public string Phone2 { get; set; }

        [Display(Name = "تلفن همراه")]
        public string Mobile { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }

        [Display(Name = "قطع همکاری")]
        public bool Discontinued { get; set; }

        public int CustomerCount { get; set; }

        public int SortOrder { get; set; }

        //IEnumerables

        [Display(Name = "مشتریان")]
        public IEnumerable<CustomerView> Customers { get; protected set; }
    }
}
