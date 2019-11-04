using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class CallLogBusinessRule
    {
        public static readonly BusinessRule CustomerRequired=new BusinessRule("Customer","مشتری باید انتخاب شود");
        public static readonly BusinessRule LocalPhoneRequired=new BusinessRule("LocalPhone","داخلی کارمند باید مشخص شود");

    }
}
