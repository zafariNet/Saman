#region Usings

using Infrastructure.Domain;
using Model.Base;
using Model.Customers;
using Model.Support.Validations;

#endregion

namespace Model.Support
{
    public class Problem : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مشتری مشکل دار
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// عنوان مشکل
        /// </summary>
        public virtual string ProblemTitle { get; set; }
        /// <summary>
        /// شرح مشکل
        /// </summary>
        public virtual string ProblemDescription { get; set; }
        /// <summary>
        /// اهمیت مشکل:
        ///1:پایین
        ///2: متوسط
        ///3: بالا
        /// </summary>
        public virtual short Priority { get; set; }
        /// <summary>
        /// وضعیت:
        ///1: باز
        ///2: در دست اقدام
        ///3: بسته
        /// </summary>
        public virtual short State { get; set; }

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Customer == null)
                base.AddBrokenRule(ProblemBusinessRules.CustomerRequired);
            if (this.ProblemTitle == null)
                base.AddBrokenRule(ProblemBusinessRules.ProblemTitleRequired);
        }
        #endregion
    }
}
