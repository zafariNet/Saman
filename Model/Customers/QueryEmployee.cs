using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;
using Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Customers
{
    public class QueryEmployee:EntityBase,IAggregateRoot
    {

        /// <summary>
        /// نما
        /// </summary>
        public virtual Query Query { get; set; }
        /// <summary>
        /// کارمند
        /// </summary>
        public virtual Employee Employee { get; set; }

        #region Validation
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Query == null)
                base.AddBrokenRule(QueryEmployeeBusinessRules.QueryRequired);
            if (this.Employee == null)
                base.AddBrokenRule(QueryEmployeeBusinessRules.EmployeeRequired);
        }

        #endregion

        // متدهای مساوی و نامساوی را بری این موجودیت بازنویسی می کنیم
        #region Methods
        /// <summary>
        /// برابر بودن دو موجودیت
        /// </summary>
        /// <param name="entity">موجودیت</param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            return entity != null
                && entity is QueryEmployee
                && this == (QueryEmployee)entity;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        /// <summary>
        /// عملگر تساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator ==(QueryEmployee entity1, QueryEmployee entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.Employee.ID.ToString() == entity2.Employee.ID.ToString() &&
                entity1.Query.ID.ToString() == entity2.Query.ID.ToString())
                return true;

            return false;
        }

        /// <summary>
        /// عملگر نامساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator !=(QueryEmployee entity1,
            QueryEmployee entity2)
        {
            return !(entity1 == entity2);
        }
        #endregion

    }
}
