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
    /// موجودیت شهر
    /// </summary>
    /// <remarks>
    /// این موجودیت توسط ظفری ایجاد شده است
    /// </remarks>
    public class City :EntityBase,IAggregateRoot
    {
        ///<summary>
        ///نام شهر
        ///</summary>
        ///<value>
        ///این خصوصیت نام شهر را برمیگرداند
        /// </value>
        public virtual string CityName { get; set; }
        ///<summary>
        ///استان
        ///</summary>
        ///<value>
        ///این خصوصیت حاوی کد استان مربوط به این شهر میباشد
        /// </value>
        public virtual Province Province { get; set; }
        ///<summary>
        ///مشتریان
        ///</summary>
        ///<value>
        ///اینم خصوصیت مشتریان در این استان را برمیگرداند
        /// </value>
        /// 
        public virtual IEnumerable<Customer> Customers { get; set; }
        ///<summary>
        ///اعتبارسنجی
        ///</summary>
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
