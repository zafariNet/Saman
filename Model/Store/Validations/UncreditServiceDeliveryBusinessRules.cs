using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class UncreditServiceDeliveryBusinessRules
    {
        public static readonly BusinessRule UncreditServiceRequired = new BusinessRule("UncreditService", "نام نمایندگی باید وارد شود");
    }
}
