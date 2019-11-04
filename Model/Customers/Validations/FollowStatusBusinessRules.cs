using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class FollowStatusBusinessRules
    {
        public static readonly BusinessRule FollowStatusNameRequired = new BusinessRule("FollowStatusName", "وضعیت پیگیری باید وارد شود");
    }
}
