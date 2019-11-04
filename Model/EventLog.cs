using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using System.Xml;

namespace Model
{
    public class EventLog : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نوع
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public virtual string URL { get; set; }
        /// <summary>
        /// XML
        /// </summary>
        public virtual XmlDocument ObjectData { get; set; }
        /// <summary>
        /// آی پی کاربر
        /// </summary>
        public virtual string UserIP { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
