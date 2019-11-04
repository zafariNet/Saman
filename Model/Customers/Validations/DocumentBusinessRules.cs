using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class DocumentBusinessRules
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule ReceiptDateRequired = new BusinessRule("ReceiptDate", "تاریخ دریافت مدرک باید وارد شود");
    }
}
