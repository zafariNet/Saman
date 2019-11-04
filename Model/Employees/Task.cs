using Infrastructure.Domain;
using Model.Base;
using Model.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees.Validations;

namespace Model.Employees
{
    public class Task:EntityBase,IAggregateRoot
    {
        #region Virtual Properties

        /// <summary>
        /// وظیفه اصلی
        /// </summary>
        public virtual Task ToDo { get; set; }

        /// <summary>
        /// عنوان وظیفه
        /// </summary>
        public virtual string ToDoTitle { get; set; }

        /// <summary>
        /// توضیحات وظیفه
        /// </summary>
        public virtual string ToDoDescription { get; set; }

        /// <summary>
        /// آیا وظیفه اصلی است؟
        /// </summary>
        public virtual bool IsMaster { get; set; }

        /// <summary>
        /// کارمند ارجاع شونده
        /// </summary>
        public virtual Employee ReferedEmployee { get; set; }

        /// <summary>
        /// مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// بسته اصلی
        /// </summary>
        public virtual bool PrimaryClosed { get; set; }

        /// <summary>
        /// تاریخ بسته شدن
        /// </summary>
        public virtual string PrimaryClosedDate { get; set; }

        /// <summary>
        /// بسته فرعی
        /// </summary>
        public virtual bool SecondaryClosed { get; set; }
        /// <summary>
        /// تاریخ بسته شدن اصلی
        /// </summary>
        public virtual string SecondaryClosedDate { get; set; }

        /// <summary>
        /// توضیحات انجام دهنده
        /// </summary>
        public virtual string ToDoResultDescription { get; set; }

        /// <summary>
        /// فایل اصلی
        /// </summary>
        public virtual string PrimaryFile { get; set; }

        /// <summary>
        /// فایل فرعی
        /// </summary>
       
        public virtual string SecondaryFile { get; set; }

        /// <summary>
        /// وظایف زیر مجموعه
        /// </summary>
        public virtual IEnumerable<Task> ToDoResults { get; set; }

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public virtual string StartDate { get; set; }

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public virtual string EndDate { get; set; }

        /// <summary>
        /// ساعت شروع
        /// </summary>
        public virtual string StartTime { get; set; }

        /// <summary>
        /// ساعت پایان
        /// </summary>
        public virtual string EndTime { get; set; }

        /// <summary>
        /// هشدار
        /// </summary>
        public virtual bool Reminder { get; set; }

        /// <summary>
        /// زمان هشدار
        /// </summary>
        public virtual string RemindTime { get; set; }

        /// <summary>
        /// ارسال پیامک
        /// </summary>
        public virtual bool SendSms { get; set; }

        #endregion




        public virtual bool Completed
        {
            get { return IsMaster && ToDoResults.Count(x => !x.SecondaryClosed)==0; }
        }

        /// <summary>
        /// امکان ویرایش جزئیات
        /// </summary>
        public virtual bool CanEditDetail { get; set; }

        /// <summary>
        /// امکان ویرایش وظیفه اصلی
        /// </summary>
        public virtual bool CanEditMaster { get; set; }

        //    get { return ToDoResults.Any(x => x.ReferedEmployee.ID == this.CreateEmployee.ID && !Completed); }
        //}

        protected override void Validate()
        {
            if (!IsMaster && ToDo == null)
                base.AddBrokenRule(TaskBusinessRule.MasterTaskRequired);
            if (string.IsNullOrEmpty(ToDoTitle))
                base.AddBrokenRule(TaskBusinessRule.TaskTitleRequired);
            if (string.IsNullOrEmpty(ToDoDescription))
                base.AddBrokenRule(TaskBusinessRule.TaskDecriptionRequired);
            //if(this.Employee==null)
            //    base.AddBrokenRule(TaskBusinessRule.EmployeeRequired);
            if(SecondaryClosed && string.IsNullOrEmpty(ToDoResultDescription))
                base.AddBrokenRule(TaskBusinessRule.SecondaryDescriptionRequired);
        }
    }
}
