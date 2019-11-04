using Infrastructure.Domain;
using Model.Base;
using Model.Employees;
using Model.Leads.Validation;

namespace Model.Leads
{
    /// <summary>
    /// موجودیت قالب های نتیجه
    /// </summary>
    public class LeadResultTemplate:EntityBase,IAggregateRoot
    {
        /// <summary>   
        /// عنوان قالب نتیجه
        /// </summary>
        public virtual string LeadResulTitle { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// گروه
        /// </summary>
        public virtual Group Group { get; set; }

        protected override void Validate()
        {
            if(string.IsNullOrEmpty(LeadResulTitle))
                AddBrokenRule(LeadResultTemplateBusinessRule.LeatResulTitleRequired);
        }
    }
}
