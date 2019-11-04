using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class ProductDeliveryBusinessRules
    {
        public static readonly BusinessRule ProductRequired = new BusinessRule("Product", "نام نمایندگی باید وارد شود");
    }
}
