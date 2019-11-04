using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Fiscals.Validations
{
    public class FiscalBusinessRules
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule MoneyAccountRequired = new BusinessRule("MoneyAccount", "حساب پولی باید وارد شود");
    }
}
