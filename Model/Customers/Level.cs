using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;
using Model.Employees;

namespace Model.Customers
{
    /// <summary>
    /// مرحله
    /// </summary>
    public class Level : EntityBase, IAggregateRoot
    {
        public Level()
        {
            GraphicalObjectProperties = new GraphicalProperties();
        }
        /// <summary>
        /// نوع مرحله
        /// </summary>
        public virtual LevelType LevelType { get; set; }
        /// <summary>
        /// عنوان مرحله که بصورت فارسی بوده و قابل نمایش به کاربر می باشد
        /// </summary>
        public virtual string LevelTitle { get; set; }
        /// <summary>
        /// اسم مستعار
        /// </summary>
        public virtual string LevelNikname { get; set; }
        /// <summary>
        /// مشخص می کند که آیا هنگام ورود اس ام اس فرستاده شود یا خیر
        /// </summary>
        public virtual bool OnEnterSendSMS { get; set; }
        /// <summary>
        /// مشخص می کند که آیا هنگام ورود ایمیل فرستاده شود یا خیر
        /// </summary>
        public virtual bool OnEnterSendEmail { get; set; }
        /// <summary>
        /// رویداد ورود به مرحله که کوئری می باشد
        /// </summary>
        public virtual string OnEnter { get; set; }
        /// <summary>
        /// رویداد هنگام خروج از مرحله که کوئری می باشد
        /// </summary>
        public virtual string OnExit { get; set; }
        /// <summary>
        /// متن ایمیلی که هنگام رسیدن مشتری به این مرحله به مشتری فرستاده می شود
        /// </summary>
        public virtual string EmailText { get; set; }
        /// <summary>
        /// متن پیام کوتاهی که هنگام رسیدن مشتری به این مرحله به مشتری فرستاده می شود
        /// </summary>
        public virtual string SMSText { get; set; }
        /// <summary>
        /// آیا این مرحله از مراحلی است که بعد از ثبت سریع مشتری وارد آن می شود؟
        /// </summary>
        public virtual bool IsFirstLevel { get; set; }
        /// <summary>
        /// آپشنهای مرحله که بصورت ایکس.ام.ال ذخیره می شود
        /// </summary>
        public virtual LevelOptions Options { get; set; }
        /// <summary>
        /// قطع
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// مسئول مرحله
        /// </summary>
        public virtual Employee LevelStaff { get; set; }
        /// <summary>
        /// آیا نیاز به تعیین شبکه در این مرحله وجود دارد؟
        /// </summary>
        public virtual bool HasRequireNetwork { get; set; }
        /// <summary>
        /// مرتب سازی
        /// </summary>
        public virtual int SortOrder { get; set; }

       

        /// <summary>
        /// نت ها
        /// </summary>
        public virtual IEnumerable<Note> Notes { get; protected set; }
        /// <summary>
        /// مرحله های بعد
        /// </summary>
        public virtual IEnumerable<LevelLevel> RelatedLevels { get; set; }

        /// <summary>
        /// مراحل بعدی
        /// </summary>
        public virtual IEnumerable<Level> NextLevels()
        {
                if (RelatedLevels != null)
                    return RelatedLevels.Select(s => s.RelatedLevel);
                else
                    return null;
        }

        /// <summary>
        /// خصوصیات نمایش حالت گرافیکی 
        /// </summary>
        public virtual GraphicalProperties GraphicalObjectProperties { get; set; }

        public virtual IEnumerable<LevelCondition> LevelConditions { get; protected set; }
        /// <summary>
        /// شرط های ورود به مرحله
        /// </summary>
        public virtual IEnumerable<Condition> Conditions
        {
            get
            {
                return LevelConditions.Select(s => s.Condition);
            }
        }

        /// <summary>
        /// ایجاد پشتیبانی به محض ورود کاربر به این مرحله
        /// </summary>
        public virtual bool CreateSupportOnEnter { get; set; }

        #region Validation

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.LevelType == null)
                base.AddBrokenRule(LevelBusinessRules.LevelTypeRequired);
            if (this.LevelTitle == null)
                base.AddBrokenRule(LevelBusinessRules.LevelTitleRequired);
            if (this.LevelNikname == null)
                base.AddBrokenRule(LevelBusinessRules.LevelNiknameRequired);
        }

        #endregion

    }
}
