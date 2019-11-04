using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public class SuctionModeDetail : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// جزئیات شیوه جذب
        /// </summary>
        public virtual string SuctionModeDetailName { get; set; }
        /// <summary>
        /// غیرفعال بودن شیوه جذب
        /// </summary>
        public virtual bool Discontinued { get; set; }
        /// <summary>
        /// مرتب سازی
        /// </summary>
        public virtual int SortOrder { get; set; }
        /// <summary>
        /// شیوه جذب والد
        /// </summary>
        public virtual SuctionMode SuctionMode { get; set; }

        //public virtual IEnumerable<Customer> Customers { get; protected set; }

        protected override void Validate()
        {
            //if (this.SuctionModeDetailName == null)
            //{
            //    base.AddBrokenRule(SuctionModeDetailBusinessRules.SuctionModedetailNameRequired);
            //}
        }

    }
}
