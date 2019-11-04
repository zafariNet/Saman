using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportStatusBusinessRule
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule SupprotStatusNameReqired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule SmsTextRequired = new BusinessRule("SmsText", "در صورت انتخاب ارسال اس  ام اس متن پیامک باید وارد شود");
        public static readonly BusinessRule EmailTextRequired = new BusinessRule("EmailText", "در صورت انتخاب ارسال ایمیل متن ایمیل باید وارد شود");
    }
}
