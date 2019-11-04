using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class StoreProductBusinessRules
    {
        public static readonly BusinessRule ProductRequired = new BusinessRule("Product", "نام کالا باید وارد شود");
        public static readonly BusinessRule StoreRequired = new BusinessRule("Store", "نام انبار باید وارد شود");
    }
}
