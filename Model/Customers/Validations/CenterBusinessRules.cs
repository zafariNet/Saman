using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class CenterBusinessRules
    {
        public static readonly BusinessRule CenterNameRequired = new BusinessRule("CenterName", "نام مرکز باید وارد شود");
    }
}
