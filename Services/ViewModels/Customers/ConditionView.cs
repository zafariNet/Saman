using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Customers
{
    public class ConditionView : BaseView
    {
        [Display(Name = "عنوان شرط")]
        public string ConditionTitle { get; set; }

        [Display(Name = "متن کوئری")]
        public string QueryText { get; set; }

        [Display(Name = "نام خصوصیت")]
        public string PropertyName { get; set; }

        [Display(Name = "مقدار")]
        public string Value { get; set; }

        [Display(Name = "اپراتور")]
        public short CriteriaOperator { get; set; }

        [Display(Name = "نمایش اپراتور")]
        public string CriteriaOperatorDisplay { get; set; }

        [Display(Name = "پیغام خطا")]
        public string ErrorText { get; set; }

        [Display(Name = "If nHibernate")]
        public bool nHibernate { get; set; }
    }
}
