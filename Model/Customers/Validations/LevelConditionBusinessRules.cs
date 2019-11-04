using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class LevelConditionBusinessRules
    {
        public static readonly BusinessRule LevelRequired = new BusinessRule("Level", "مرحله مشتری باید وارد شود");
        public static readonly BusinessRule ConditionRequired = new BusinessRule("Condition", "شرط باید وارد شود");
    }
}
