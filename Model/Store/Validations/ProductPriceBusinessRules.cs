using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class ProductPriceBusinessRules
    {
        public static readonly BusinessRule ProductRequired = new BusinessRule("Product", "نام کالا باید وارد شود.");
        public static readonly BusinessRule ProductPriceTitleRequired = new BusinessRule("ProductPriceTitle", "شرح باید وارد شود.");
        public static readonly BusinessRule ProductPriceCodeRequired = new BusinessRule("ProductPriceCode", "کد محصول باید وارد شود");
    }
}
