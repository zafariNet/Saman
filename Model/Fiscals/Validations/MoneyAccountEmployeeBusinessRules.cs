using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Fiscals.Validations
{
    public class MoneyAccountEmployeeBusinessRules
    {
        public static readonly BusinessRule EmployeeRequired = new BusinessRule("Employee", "کارمند باید وارد شود");
        public static readonly BusinessRule MoneyAccountRequired = new BusinessRule("MoneyAccount", "حساب مالی جهت تأیید دریافت باید وارد شود");
    }
}
