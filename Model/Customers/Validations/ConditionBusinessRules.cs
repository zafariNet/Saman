using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class ConditionBusinessRules
    {
        public static readonly BusinessRule ConditionTitleRequired = new BusinessRule("ConditionTitle", "عنوان شرط باید وارد شود");
        public static readonly BusinessRule QueryTextRequired = new BusinessRule("QueryText", "متن کوئری مربوط به شرط باید وارد شود");
        public static readonly BusinessRule PropertyNameRequired = new BusinessRule("PropertyName", "نام پارامتر باید وارد شود");
    }
}
