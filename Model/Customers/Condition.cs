using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class Condition : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// عنوان شرط
        /// </summary>
        public virtual string ConditionTitle { get; set; }
        /// <summary>
        /// کوئری مربوط به شرط
        /// </summary>
        public virtual string QueryText { get; set; }
        /// <summary>
        /// نام خصوصیت
        /// </summary>
        public virtual string PropertyName { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// پیش شماره
        /// </summary>
        public virtual short CriteriaOperator { get; set; }
        /// <summary>
        /// پیغام خطایی که هنگام صحیح نبودن شرط نمایش داده میشود
        /// </summary>
        public virtual string ErrorText { get; set; }
        /// <summary>
        /// مقدار این خصوصیت تعیین کننده این است که شرط از طریق هایبرنیت اجرا می شود یا خیر
        /// </summary>
        public virtual bool nHibernate { get; set; }

        #region Validate

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.ConditionTitle == null)
                base.AddBrokenRule(ConditionBusinessRules.ConditionTitleRequired);
            if (this.QueryText == null)
                base.AddBrokenRule(ConditionBusinessRules.QueryTextRequired);
            //if (this.PropertyName == null)
            //    base.AddBrokenRule(ConditionBusinessRules.PropertyNameRequired);
        }

        #endregion
    }
}
