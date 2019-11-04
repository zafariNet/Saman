using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Leads.Validation
{
    public class LeadResultTemplateBusinessRule
    {
        public static readonly BusinessRule LeatResulTitleRequired = new BusinessRule("LeatResulTitle","عنوان قالب نتیجه تماس باید وارد شود.");
    }
}
