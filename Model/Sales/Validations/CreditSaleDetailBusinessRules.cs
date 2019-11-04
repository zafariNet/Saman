using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Sales.Validations
{
    public class CreditSaleDetailBusinessRules
    {
        public static readonly BusinessRule SaleRequired = new BusinessRule("Sale", "مشخصات اصلی صورتحساب باید وارد شود");
        public static readonly BusinessRule CreditServiceRequired = new BusinessRule("CreditService", "خدمات اعتباری باید وارد شود");
        public static readonly BusinessRule MainCreditSaleDetailRequired = new BusinessRule("MainCreditSaleDetail", "جزییات فاکتور اصلی باید وارد شود");
    }
}
