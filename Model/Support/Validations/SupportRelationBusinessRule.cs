using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportRelationBusinessRule
    {
        public static readonly BusinessRule SupportStatusRequired = new BusinessRule("SupportStatus", "سطح باید وارد شود");
        public static readonly BusinessRule RelatedSupportStatusRequired = new BusinessRule("RelatedSupportStatus", "سطح باید وارد شود");
    }
}
