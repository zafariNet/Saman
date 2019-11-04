using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Sales.Validations
{
    public class ProductSaleDetailBusinessRules
    {
        public static readonly BusinessRule SaleRequired = new BusinessRule("Sale", "صورتحساب باید وارد شود");
        public static readonly BusinessRule ProductRequired = new BusinessRule("Product", "کالا باید وارد شود");
    }
}
