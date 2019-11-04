using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class SuctionModeBusinessRules
    {
        public static readonly BusinessRule SuctionModeNameRequired = new BusinessRule("SuctionModeName", "شیوه جذب باید وارد شود");
    }
}
