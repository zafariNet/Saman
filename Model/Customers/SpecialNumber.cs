using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    /// <summary>
    /// شماره های خاص
    /// هر مرکز مخابراتی می تواند شماره هایی داشته باشد که بصورت خاص تحت پوشش نیستند. این شماره ها شامل یک رنج خاص می باشند مثلا از 77541100 تا 77541200
    /// </summary>
    public class SpecialNumber : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// از شماره
        /// </summary>
        public virtual int FromNumber { get; set; }
        /// <summary>
        /// تا شماره
        /// </summary>
        public virtual int ToNumber { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {

        }  
    }
}
