using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class NetworkBusinessRules
    {
        public static readonly BusinessRule NetworkNameRequired = new BusinessRule("NetworkName", "نام نمایندگی باید وارد شود");
    }
}
