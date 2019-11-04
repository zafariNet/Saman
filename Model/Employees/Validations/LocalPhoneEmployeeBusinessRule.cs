using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees.Validations
{
    public class LocalPhoneEmployeeBusinessRule
    {
        public static readonly BusinessRule LocalPhoneStoreRequired = new BusinessRule("LocalPhoneStore", "شماره داخلی باید وارد شود");
        public static readonly BusinessRule OwnerEmployeeRequired = new BusinessRule("OwnerEmployee", "کارمند باید مشخص شود");
    }
}
