using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Lead.Validation
{
    public class LeadTitleTemplateBusinessRule
    {
        public static readonly BusinessRule TitleRequired = new BusinessRule("Title","وارد کردن عنوان قالب اجباریست.");
    }
}
