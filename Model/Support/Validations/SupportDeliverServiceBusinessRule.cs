using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportDeliverServiceBusinessRule
    {
        public static readonly BusinessRule SupportRequired = new BusinessRule("Support", "پشتیبانی باید وارد شود");
        public static readonly BusinessRule DeliverDateRequired = new BusinessRule("DeliverDate", "تاریخ تحویل باید وارد شود");
        public static readonly BusinessRule TimeInputRequired = new BusinessRule("TimeInput", "زمان ورود کاشناس باید وارد شود");
        public static readonly BusinessRule TimeOutputRequired = new BusinessRule("TimeOutput", "زمان خروج کاشناس باید وارد شود");
        public static readonly BusinessRule CommentRequired = new BusinessRule("Comment", "توضیحات باید وارد شود");
    }
}
