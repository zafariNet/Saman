using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model.Support
{
    public class SupportStatusRelation :EntityBase,  IAggregateRoot
    {
        /// <summary>
        /// وضعیتی که یک پشتیبانی دارد
        /// </summary>
        public virtual SupportStatus SupportStatus { get; set; }
        /// <summary>
        /// وضعیتی که پشتیبانی به آن ارسال میشود    
        /// </summary>
        public virtual SupportStatus RelatedSupportStatus { get; set; }


        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
