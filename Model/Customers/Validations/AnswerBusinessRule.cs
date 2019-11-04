using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class AnswerBusinessRule
    {
        public static readonly BusinessRule AnswerTextRequired=new BusinessRule("AnswerText","وارد کردن متن جواب الزامیست.");
        public static readonly BusinessRule QuestionRequired = new BusinessRule("Question", "انتخواب وسال الزامیست.");
    }
}
