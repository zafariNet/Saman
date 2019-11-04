using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class LevelType : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// عنوان مرحله
        /// </summary>
        public virtual string Title{get;set;}

        /// <summary>
        /// مرحله ها
        /// </summary>
        public virtual IEnumerable<Level> Levels { get; protected set; }



        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Title == null)
                base.AddBrokenRule(LevelTypeBusinessRules.TitleRequired);
        }  
    }
}
