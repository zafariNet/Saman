using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class QuestionBuisinessRule
    {
        public static readonly BusinessRule QuestionTextRequired=new BusinessRule("QuestionText","وارک کردن عنوان سوال الزامیست.");
        public static readonly BusinessRule LevelRequired=new BusinessRule("Level","انتخاب مرحله الزامیست.");
    }
}
