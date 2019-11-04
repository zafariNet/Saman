using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model
{
    /// <summary>
    /// در این موجودیت میتوان عبارات فارسی با معادل مختصر انگلیسی ذخیره کرد که در صورت نیاز به استفاده از عبارت فارسی در پروسیجرها و فانکشنها مجبور به تایپ این عبارات در متن پروسیجر نباشیم
    /// این امکان زمان کارآمد است که بخواهیم دیتابیس را در یک سرور عمومی که فارسی را پشتیبانی نمی کند اجرا کنیم
    /// </summary>
    public class TextConvert : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// معادل انگلیسی
        /// </summary>
        public virtual string EnText { get; set; }
        /// <summary>
        /// معادل فارسی
        /// </summary>
        public virtual string FaText { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
