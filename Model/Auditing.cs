using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model
{
    public class Auditing : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نوع رویداد
        /// </summary>
        public virtual string EventType { get; set; }
        /// <summary>
        /// زمان ارسال
        /// </summary>
        public virtual DateTime PostTime { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public virtual string LoginName { get; set; }
        /// <summary>
        /// متن دستور
        /// </summary>
        public virtual string CommandText { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
