using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class QueryEmployeeBusinessRules
    {
        public static readonly BusinessRule QueryRequired = new BusinessRule("Query", "نما باید وارد شود");
        public static readonly BusinessRule EmployeeRequired = new BusinessRule("Employee", "کارمند باید وارد شود");
    }
}
