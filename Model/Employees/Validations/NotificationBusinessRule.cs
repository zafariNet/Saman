using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class NotificationBusinessRule
    {
        public static readonly BusinessRule NotificationTitleRequired=new BusinessRule("NotificationTitle","عنوان پیام باید وارد شود");
        public static readonly BusinessRule NotificationCommentRequired = new BusinessRule("NotificationTitle", "متن پیام باید وارد شود");
    }
}
