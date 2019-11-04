using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class SmsEmployeeBusinessRule
    {
        public static readonly BusinessRule EmployeeReqired=new BusinessRule("Employee","کارمد گیرنده پیامک مشخص نیست");
        public static readonly BusinessRule BodyReqired=new BusinessRule("Body","متن پیامک باید وارد شود");
    }
}
