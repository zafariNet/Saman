using Infrastructure.Domain;
using Model.Base;
using Model.Customers;
using Model.Employees;
using Model.Leads.Validation;

namespace Model.Leads
{
    #region Enum

    public enum NegotiationStatuses
    {
        Closed,         //بسته شده
        ReferAndClosed,     // ارجاع شده و بسته شده
        Refered     // ارجاع شده

    }
    #endregion

    /// <summary>
    /// موجودیت مذاکره فروش
    /// </summary>
    public class Negotiation:EntityBase,IAggregateRoot
    {
        /// <summary>
        /// کاربر انجام دهنده
        /// </summary>
        public virtual Employee ReferedEmployee { get; set; }
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// عنوان مذاکره
        /// </summary>
        public virtual LeadTitleTemplate LeadTitleTemplate { get; set; }
        /// <summary>
        /// توضیحات مذاکره
        /// </summary>
        public virtual string NegotiationDesciption { get; set; }
        /// <summary>
        /// تاریخ مذاکره
        /// </summary>
        public virtual string NegotiationDate { get; set; }
        /// <summary>
        /// زمان مذاکره
        /// </summary>
        public virtual string NegotiationTime { get; set; }

        /// <summary>
        /// زمان یادآوری
        /// </summary>
        public virtual string RememberTime { get; set; }
        /// <summary>
        /// ارسال پیامک یاداوری
        /// </summary>
        public virtual bool SendSms { get; set; }
        /// <summary>
        /// قالب نتیجه
        /// </summary>
        public virtual LeadResultTemplate LeadResultTemplate { get; set; }
        /// <summary>
        /// توضیحات نتیجه
        /// </summary>
        public virtual string NeqotiationResultDescription { get; set; }
        /// <summary>
        /// تاریخ بسته شدن
        /// </summary>
        public virtual string CloseDate { get; set; }
        /// <summary>
        /// بسته شده
        /// </summary>
        public virtual bool Closed { get; set; }

        public virtual NegotiationStatuses NegotiationStatus { get; set; }
        

        protected override void Validate()
        {
            if(ReferedEmployee==null)
                AddBrokenRule(NegotioationBusinessRule.ReferedEmployeeRequired);
            if(Customer==null)
                AddBrokenRule(NegotioationBusinessRule.CustomerRequired);
            if(LeadTitleTemplate==null)
                AddBrokenRule(NegotioationBusinessRule.LeadTitleTemplateRequired);
            if(string.IsNullOrEmpty(NegotiationDate))
                AddBrokenRule(NegotioationBusinessRule.NegotiationDateRequired);
            if(string.IsNullOrEmpty(NegotiationTime))
                AddBrokenRule(NegotioationBusinessRule.NegotiationTimeRequired);
            if(Closed && LeadResultTemplate==null)
                AddBrokenRule(NegotioationBusinessRule.LeadResultTemplateRequired);
        }
    }
}
