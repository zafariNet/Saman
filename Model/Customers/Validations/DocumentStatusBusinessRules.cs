using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class DocumentStatusBusinessRules
    {
        public static readonly BusinessRule DocumentStatusNameRequired = new BusinessRule("DocumentStatusName", "وضعیت مدارک باید وارد شود");
    }
}
