using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class ProductLogBusinessRules
    {
        public static readonly BusinessRule ProductRequired = new BusinessRule("Product", "نام کالا باید وارد شود");
        public static readonly BusinessRule LogDateRequired = new BusinessRule("LogDate", "تاریخ تراکنش باید وارد شود");
    }
}
