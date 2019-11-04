using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class NetworkCreditBusinessRules
    {
        public static readonly BusinessRule NetworkRequired = new BusinessRule("Network", "نام نمایندگی باید وارد شود");
    }
}
