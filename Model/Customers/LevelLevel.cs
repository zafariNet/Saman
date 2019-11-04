#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

#endregion

namespace Model.Customers
{
    public class LevelLevel : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مرحله ای که مشتری در آن قرار دارد
        /// </summary>
        public virtual Level Level { get; set; }
        /// <summary>
        /// مرحله ای که مشتری به آن فرستاده میشود
        /// </summary>
        public virtual Level RelatedLevel { get; set; }

        #region Validation

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Level == null)
                base.AddBrokenRule(LevelLevelBusinessRules.LevelRequired);
            if (this.RelatedLevel == null)
                base.AddBrokenRule(LevelLevelBusinessRules.RelatedLevelRequired);
        }
                
        #endregion
    }
}
