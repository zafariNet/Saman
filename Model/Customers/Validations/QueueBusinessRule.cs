using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class QueueBusinessRule
    {
        public static readonly BusinessRule QueueKeyRequired = new BusinessRule("Queuekey","نام صف باید وارد شود");
        public static readonly BusinessRule QueueEployeeRequired = new BusinessRule("QueueEmployee", "کارمند باید مشخص شود.");
    }
}
