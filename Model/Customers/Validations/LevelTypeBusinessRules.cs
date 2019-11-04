using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class LevelTypeBusinessRules
    {
        public static readonly BusinessRule TitleRequired = new BusinessRule("Title", "عنوان باید وارد شود");
    }
}
