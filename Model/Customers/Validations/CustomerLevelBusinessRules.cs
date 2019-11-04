using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class CustomerLevelBusinessRules
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule LevelRequired = new BusinessRule("Level", "سطح باید وارد شود");
    }
}
