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
    public class CityView:BaseView
    {
        [Display(Name = "نام شهر")]
        public string CityName { get; set; }
        [Display(Name = "استان")]
        public ProvinceView Province { get; set; }
        [Display(Name = "تعداد مشتریان شهر")]
        public string CustomerCount { get; set; }
        [Display(Name = "مشتریان شهر")]
        public IEnumerable<CustomerView> Customers { get; set; }
    }
}
