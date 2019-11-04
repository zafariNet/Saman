using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class LevelCondition : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مرحله
        /// </summary>
        public virtual Level Level { get; set; }
        /// <summary>
        /// شرط
        /// </summary>
        public virtual Condition Condition { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Level == null)
                base.AddBrokenRule(LevelConditionBusinessRules.LevelRequired);
            if (this.Condition == null)
                base.AddBrokenRule(LevelConditionBusinessRules.ConditionRequired);
        }  
    }
}
