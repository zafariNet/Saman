using System.Collections.Generic;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class Question:EntityBase,IAggregateRoot
    {
        #region Properties

        /// <summary>
        /// متن سوال
        /// </summary>
        public virtual string QuestionText { get; set; }
        /// <summary>
        /// جواب های این سوال
        /// </summary>
        public virtual IEnumerable<Answer> Answers { get; set; }
        /// <summary>
        /// مرحله
        /// </summary>
        public virtual Level Level { get; set; }
        #endregion

        #region Validation

        protected override void Validate()
        {
            if(string.IsNullOrEmpty(QuestionText))
                AddBrokenRule(QuestionBuisinessRule.QuestionTextRequired);
            if(Level==null)
                AddBrokenRule(QuestionBuisinessRule.LevelRequired);
        }

        #endregion
    }
}
