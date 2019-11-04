using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model.Customers
{
    /// <summary>
    /// موجودیت سوال و جواب
    /// </summary>
    public class QuestionAnswer:EntityBase,IAggregateRoot
    {
        #region Properties

        /// <summary>
        /// سوال
        /// </summary>
        public virtual Answer Answer { get; set; }
        /// <summary>
        /// جواب
        /// </summary>
        public virtual Question Question { get; set; }
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }

        #endregion

        #region Validation

        protected override void Validate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
