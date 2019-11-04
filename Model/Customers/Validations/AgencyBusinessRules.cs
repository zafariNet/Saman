using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class AgencyBusinessRules
    {
        public static readonly BusinessRule AgencyNameRequired = new BusinessRule("AgencyName", "نام نمایندگی باید وارد شود");
    }
}
