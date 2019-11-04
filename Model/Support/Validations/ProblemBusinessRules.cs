using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class ProblemBusinessRules
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule ProblemTitleRequired = new BusinessRule("ProblemTitle", "عنوان مشکل باید وارد شود");
    }
}
