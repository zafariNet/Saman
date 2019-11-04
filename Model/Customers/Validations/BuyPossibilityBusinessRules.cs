using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class BuyPossibilityBusinessRules
    {
        public static readonly BusinessRule BuyPossibilityNameRequired = new BusinessRule("BuyPossibilityName", "وضعیت پیگیری باید وارد شود");
    }
}
