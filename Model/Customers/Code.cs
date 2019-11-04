using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class Code : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// پیش شماره
        /// </summary>
        public virtual string CodeName { get; set; }
        /// <summary>
        /// مرکز
        /// </summary>
        public virtual Center Center{ get; set; }
        public virtual bool AddedToSite { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Center == null)
                base.AddBrokenRule(CodeBusinessRules.CenterRequired);
            if (this.CodeName == null)
                base.AddBrokenRule(CodeBusinessRules.CodeNameRequired);
            if (this.CodeName.Length < 5 || this.CodeName.Length > 5)
                base.AddBrokenRule(CodeBusinessRules.CodeMustBeGraterThan5Character);
        }  
    }
}
