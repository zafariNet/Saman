using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Sales;

namespace Model.Employees
{
    /// <summary>
    /// موجودیت کارمند پیک
    /// </summary>
    public class CourierEmployee : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public virtual string LastName { get; set; }
        
        /// <summary>
        /// آدرس
        /// </summary>
        public virtual string Address { get; set; }
        
        /// <summary>
        /// تلفن
        /// </summary>
        public virtual string Phone { get; set; }
        
        /// <summary>
        /// موبایل
        /// </summary>
        public virtual string Mobile { get; set; }
        
        /// <summary>
        /// پیک های اعزام شده
        /// </summary>
        public virtual IEnumerable<Courier> Couriers { get; set; }

    protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
