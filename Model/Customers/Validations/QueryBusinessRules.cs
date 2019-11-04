using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class QueryBusinessRules
    {
        public static readonly BusinessRule TitleRequired = new BusinessRule("Title", "عنوان نما باید وارد شود");
        public static readonly BusinessRule QueryTextRequired = new BusinessRule("QueryText", "متن کوئری نما باید وارد شود");
    }
}
