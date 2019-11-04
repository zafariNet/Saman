using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
namespace Model.Customers.Validations
{
    class SuctionModeDetailBusinessRules
    {
        public static readonly BusinessRule SuctionModedetailNameRequired = new BusinessRule("SuctionModeDetailName", "نام جزئیات شیوه جذب باید وارد شود");
    }
}
