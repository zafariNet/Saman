using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model.Employees
{
    /// <summary>
    /// موجودیت پیام ها
    /// </summary>
    public class Notification:EntityBase,IAggregateRoot
    {

        public enum NotificationTypes
        {
            ByEmployee,     // توسط کارمند
            BySystem        //توسط سیستم
        }
        /// <summary>
        /// عنوان پیام
        /// </summary>
        public virtual string NotificationTitle { get; set; }

        /// <summary>
        /// توضیحات پیام
        /// </summary>
        public virtual string NotificationComment { get; set; }

        /// <summary>
        /// نوع پیام
        /// </summary>
        public virtual NotificationTypes NotificationType { get; set; }

        /// <summary>
        /// ارجاع شده به کارمند
        /// </summary>
        public virtual Employee ReferedEmployee { get; set; }

        /// <summary>
        /// تاریخ مشاهده شده
        /// </summary>
        public virtual string VisitedDate { get; set; }

        public virtual bool Visited { get; set; }


        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
