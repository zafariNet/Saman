using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportQcBusinessRule
    {
        public static readonly BusinessRule SupportRequired = new BusinessRule("Support", "پشتیبانی باید وارد شود");
        public static readonly BusinessRule InputTimeRequired = new BusinessRule("InputTime", "ساعت حضور کاشناس باید وارد شود");
        public static readonly BusinessRule OutputTimeRequired = new BusinessRule("OutputTime", "ساعت خروج کارشناس باید وارد شود");
        public static readonly BusinessRule CommentRequired = new BusinessRule("Comment", "توضیحات باید وارد شود");
    }
}
