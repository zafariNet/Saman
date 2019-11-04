using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupporExpertDispatchBusinessRule
    {
        public static readonly BusinessRule SupportRequired = new BusinessRule("Support", "پشتیبانی باید وارد شود");
        public static readonly BusinessRule DispatchDateRequired = new BusinessRule("DispatchDate", "تاریخ اعزام باید وارد شود");
        public static readonly BusinessRule DispatchTimeRequired = new BusinessRule("DispatchTime", "زمان اعزام باید وارد شود");
        public static readonly BusinessRule CommentRequired = new BusinessRule("Comment", "توضیحات باید وارد شود");
    }
}
