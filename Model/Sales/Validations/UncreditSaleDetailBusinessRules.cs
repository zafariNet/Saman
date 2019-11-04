using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Sales.Validations
{
    public class UncreditSaleDetailBusinessRules
    {
        public static readonly BusinessRule SaleRequired = new BusinessRule("Sale", "فروش باید وارد شود");
        public static readonly BusinessRule UncreditServiceRequired = new BusinessRule("UncreditService", "خدمات غیراعتباری باید وارد شود");
    }
}
