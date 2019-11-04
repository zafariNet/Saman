using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees.Validations;

namespace Model.Employees
{
    public class MessageTemplate:EntityBase,IAggregateRoot
    {
        public enum  MessageTypes
        {
            Sms=1,
            Email=2
        }

        /// <summary>
        /// عنوان قالب پیام
        /// </summary>
        public virtual string MessageTemplateName { get; set; }
        /// <summary>
        /// متن قالب پیام
        /// </summary>
        public virtual string MessageEmailTemplateText { get; set; }

        public virtual string MessageSmsTemplateText { get; set; }
        /// <summary>
        /// نوع قالب
        /// </summary>
        public virtual MessageTypes MessageType { get; set; }

        protected override void Validate()
        {
            if(this.MessageTemplateName==null)
                base.AddBrokenRule(MessageTemplateBusinessRule.MessageTemplateNameRequired);
        }
    }
}
