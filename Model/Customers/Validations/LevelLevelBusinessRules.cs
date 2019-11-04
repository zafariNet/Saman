using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class LevelLevelBusinessRules
    {
        public static readonly BusinessRule LevelRequired = new BusinessRule("Level", "سطح باید وارد شود");
        public static readonly BusinessRule RelatedLevelRequired = new BusinessRule("RelatedLevel", "سطح باید وارد شود");
    }
}
