using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class Note : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// مرحله ای که یادداشت به آن تعلق دارد
        /// </summary>
        public virtual Level Level { get; set; }
        /// <summary>
        /// یادداشت
        /// </summary>
        public virtual string NoteDescription { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Customer == null)
                base.AddBrokenRule(NoteBusinessRules.CustomerRequired);
            //if (this.Level == null)
            //    base.AddBrokenRule(NoteBusinessRules.LevelRequired);
        }  
    }
}
