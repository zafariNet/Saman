using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class CodeBusinessRules
    {
        public static readonly BusinessRule CenterRequired = new BusinessRule("Center", "مرکز باید وارد شود");
        public static readonly BusinessRule CodeNameRequired = new BusinessRule("CodeName", "کد باید وارد شود");
        public static readonly BusinessRule CodeMustBeGraterThan5Character = new BusinessRule("CodeName", "پیش شماره باید  5 رقم باشد");
    }
}
