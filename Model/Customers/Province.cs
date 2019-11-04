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
    /// موجودیت استان
    /// </summary>
    /// <remarks>
    /// این موجودیت توسط محمد ظفری ایجاد شده است
    /// </remarks>
    public class Province:EntityBase,IAggregateRoot
    {
        ///<summary>
        ///نام استان
        ///</summary>
        ///<value>
        ///این خصوصیت نام استان را برمیگرداند
        /// </value>
        public virtual string ProvinceName { get; set; }
        ///<summary>
        ///شهرها
        ///</summary>
        ///<value>
        ///این خصوصیت لیست شهرهای استان را برمیگرداند
        /// </value>
        public virtual IEnumerable<City> Cities { get; set; }
        ///<summary>
        ///مشتریان
        ///</summary>
        ///<value>
        ///مشتریان در این استان را برمیپگرداند
        /// </value>
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
