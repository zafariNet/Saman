using Infrastructure.Domain;
using Model.Base;
using Model.Employees;
using Model.Lead.Validation;

namespace Model.Leads
{
    public class LeadTitleTemplate:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// عنوانی که در لیست نمایش داده میشود
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// آیا با انتخاب این قالب مذاکره بسته شود یا نه
        /// </summary>
        public virtual bool CloseLeadConversation { get; set; }
        /// <summary>
        /// گروه
        /// </summary>
        public virtual Group Group { get; set; }

        protected override void Validate()
        {
            if(string.IsNullOrEmpty(Title))
                AddBrokenRule(LeadTitleTemplateBusinessRule.TitleRequired);
        }
    }
}
