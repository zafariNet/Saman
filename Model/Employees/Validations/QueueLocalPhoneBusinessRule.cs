using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees.Validations
{
    public class QueueLocalPhoneBusinessRule
    {
        public static readonly BusinessRule LocalPhoneRequired = new BusinessRule("LocalPhone", "داخلی مورد نظر باید وارد شود");
        public static readonly BusinessRule QueueRequired = new BusinessRule("Queue", "صف مورد نظر باید وارد شود");

    }
}
