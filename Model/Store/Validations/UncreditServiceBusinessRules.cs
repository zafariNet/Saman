using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class UncreditServiceBusinessRules
    {
        public static readonly BusinessRule UncreditServiceNameRequired = new BusinessRule("UncreditServiceName", "نام نمایندگی باید وارد شود");
    }
}
