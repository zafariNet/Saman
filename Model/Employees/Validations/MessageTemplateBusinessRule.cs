using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class MessageTemplateBusinessRule
    {

        public static readonly BusinessRule MessageTemplateNameRequired = new BusinessRule("MessageTemplateName", "عنوان قالب باید وارد شود");
        public static readonly BusinessRule MessageTemplateTextRequired = new BusinessRule("MessageTemplateText", "متن قالب باید وارد شود");
    }
}
