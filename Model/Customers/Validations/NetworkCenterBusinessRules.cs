using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class NetworkCenterBusinessRules
    {
        public static readonly BusinessRule CenterRequired = new BusinessRule("Center", "مرکز مخابراتی باید وارد شود");
        public static readonly BusinessRule NetworkRequired = new BusinessRule("Network", "مرکز مخابراتی باید وارد شود");
    }
}
