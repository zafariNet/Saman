using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model
{
    public class ErrorLog : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// عکسی که از مانیتور شخص ایجاد کننده خطا در این محل ذخیره می شود
        /// </summary>
        public virtual string Pic { get; set; }
        /// <summary>
        /// تاریخ خطا
        /// </summary>
        public virtual DateTime ErrDate { get; set; }
        /// <summary>
        /// ساعت خطا
        /// </summary>
        public virtual DateTime Time { get; set; }
        /// <summary>
        /// کارمندی که خطا توسط او اتفاق افتاده است
        /// </summary>
        public virtual Employees.Employee Employee { get; set; }
        /// <summary>
        /// نام و نام خانوادگی کارمند
        /// در اینجا بصورت کامل ذخیره میشود
        /// یادم نیست چرا
        /// </summary>
        public virtual string EmployeeName { get; set; }
        /// <summary>
        /// پیام خطا
        /// </summary>
        public virtual string Message { get; set; }
        /// <summary>
        /// از مشخصات پیغام خطا (منبع خطا)
        /// </summary>
        public virtual string Source { get; set; }
        /// <summary>
        /// از مشخصات پیغام خطا
        /// </summary>
        public virtual string StackTrace { get; set; }
        /// <summary>
        /// آیا این خطا توسط ادمین سیستم بررسی شده یا خطای جدید می باشد؟
        /// </summary>
        public virtual bool Checked { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
