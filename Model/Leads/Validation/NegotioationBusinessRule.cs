using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Leads.Validation
{
    public class NegotioationBusinessRule
    {
        public static readonly BusinessRule ReferedEmployeeRequired = new BusinessRule("ReferedEmployee","کارشناس ارجاع باید مشخص شود.");
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید مشخص شود.");
        public static readonly BusinessRule LeadTitleTemplateRequired = new BusinessRule("LeadTitleTemplate", "قالب مذاکره نمیتواند خالی باشد.");
        public static readonly BusinessRule NegotiationDateRequired = new BusinessRule("NegotiationDate", "تاریخ مذاکره باید مشخص شود");
        public static readonly BusinessRule NegotiationTimeRequired = new BusinessRule("NegotiationTime", "زمان مذاکره باید مشخص شود.");
        public static readonly BusinessRule LeadResultTemplateRequired = new BusinessRule("LeadResultTemplate", "قالب نتیجه باید مشخص شود.");


    }
}
