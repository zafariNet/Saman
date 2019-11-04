using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportBusinessRule
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule SupportTitleRequired = new BusinessRule("SupportTitle", "عنوان مشکل باید وارد شود");
        public static readonly BusinessRule SupportCommentRequired = new BusinessRule("SupportComment", "توضیح مشکل باید وارد شود");
    }
}
