using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class Answer:EntityBase,IAggregateRoot
    {
        #region Properties

        /// <summary>
        /// متن جواب
        /// </summary>
        public virtual string AnswerText { get; set; }
        /// <summary>
        ///  سوال
        /// </summary>
        public virtual Question Question { get; set; }

        #endregion

        #region Validation

        protected override void Validate()
        {
            if(string.IsNullOrEmpty(AnswerText))
                AddBrokenRule(AnswerBusinessRule.AnswerTextRequired);
            if(Question==null)
                AddBrokenRule(AnswerBusinessRule.QuestionRequired);
        }
        #endregion
    }
}
