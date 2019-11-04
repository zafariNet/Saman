using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Fiscals.Validations
{
    public class MoneyAccountBusinessRules
    {
        public static readonly BusinessRule AccountNameRequired = new BusinessRule("AccountName", "نام حساب پولی باید وارد شود");
    }
}
