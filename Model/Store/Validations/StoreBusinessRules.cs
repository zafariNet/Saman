using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class StoreBusinessRules
    {
        public static readonly BusinessRule OwnerEmployeeRequired = new BusinessRule("OwnerEmployee", "نام نمایندگی باید وارد شود");
    }
}
