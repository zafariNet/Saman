using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class CreditServiceBusinessRules
    {
        public static readonly BusinessRule NetworkRequired = new BusinessRule("Network", "نام نمایندگی باید وارد شود");
        public static readonly BusinessRule ServiceNameRequired = new BusinessRule("ServiceName", "نام نمایندگی باید وارد شود");
    }
}
