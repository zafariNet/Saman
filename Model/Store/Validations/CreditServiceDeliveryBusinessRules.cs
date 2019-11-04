using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class CreditServiceDeliveryBusinessRules
    {
        public static readonly BusinessRule CreditServiceRequired = new BusinessRule("CreditService", "نام نمایندگی باید وارد شود");
    }
}
