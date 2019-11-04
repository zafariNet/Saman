using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Sales.Validations
{
    public class CourierBusinessRules
    {
        public static readonly BusinessRule SaleRequired = new BusinessRule("Sale", "فروش باید مشخص شود");
        public static readonly BusinessRule DeliverDateRequired = new BusinessRule("DeliverDate", "تاریخ اعزام پیک باید مشخص شود");
        public static readonly BusinessRule DeliverTimeRequired = new BusinessRule("DeliverTime", "ساعت اعزام پیک باید وارد شود");

    }
}
