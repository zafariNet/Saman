using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model
{
    public class GeneralSettings : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// آدرس فایل پشتیبان
        /// </summary>
        public virtual string BackupAddress { get; set; }
        /// <summary>
        /// این فیلد در نرم افزار تحت ویندوز جهت خروج فوری کاربر مورد استفاده قرار می گیرد
        /// به این صورت که هر کلاینت در فواصل زمانی معین چک می کند که آیا مقدار این فیلد صفر است یا 1 که اگر 1 باشد نرم افزار وی بسته می شود
        /// </summary>
        public virtual bool ExitImmediate { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
