using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Services.ViewModels.Customers
{
    /// <summary>
    /// ایجاد شده توسط محمد ظفری
    /// </summary>
    public class ProvinceView:BaseView
    {
        [Display(Name = "نام استان")]
        public string ProvinceName { get; set; }
        [Display(Name = "تعداد مشتریان استان")]
        public string CustomerCount { get; set; }
        [Display(Name = "شهرهای استان")]
        public IEnumerable<CityView> Cities { get; set; }
        [Display(Name = "مشتریان استان")]
        public IEnumerable<CustomerView> Customers { get; set; }
    }
}
