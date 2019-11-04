using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Model.Employees;
using Model.Base.Validations;

namespace Model.Base
{
    /// <summary>
    /// کلاس پایه که بقیه موجودیت ها از این کلاس مشتق می شوند
    /// </summary>
    public abstract class EntityBaseComplexID
    {
        #region Properties
        /// <summary>
        /// شناسه موجودیت
        /// </summary>
        public virtual Guid ID1 { get; set; }
        /// <summary>
        /// شناسه موجودیت
        /// </summary>
        public virtual Guid ID2 { get; set; }

        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public virtual string CreateDate { get; set; }
        /// <summary>
        /// کارمند ایجاد کننده
        /// </summary>
        public virtual Employee CreateEmployee { get; set; }

        /// <summary>
        /// تاریخ آخرین ویرایش
        /// </summary>
        public virtual string ModifiedDate { get; set; }
        /// <summary>
        /// کارمند ویرایش کننده
        /// </summary>
        public virtual Employee ModifiedEmployee { get; set; }

        /// <summary>
        /// ورژن رکورد - برای ویرایش همزمان به کار می رود
        /// </summary>
        public virtual int RowVersion { get; set; }
        #endregion

        #region Validation
        List<BusinessRule> _brokenRules = new List<BusinessRule>();

        protected abstract void Validate();

        private void ValidateEntityBase()
        {
            if (CreateDate == null)
                AddBrokenRule(EntityBaseBusinessRules.CreateDateRequired);

            if (!new PersianDateTimeValidationSpecification().IsSatisfiedBy(CreateDate))
                AddBrokenRule(EntityBaseBusinessRules.CreateDateIsInvalid);

            if (ModifiedDate != null && !new PersianDateTimeValidationSpecification().IsSatisfiedBy(ModifiedDate))
                AddBrokenRule(EntityBaseBusinessRules.MidifiedDateIsInvalid);
        }

        public virtual IEnumerable<BusinessRule> GetBrokenRules()
        {
            _brokenRules.Clear();
            Validate();
            ValidateEntityBase();
            return _brokenRules;
        }

        protected void AddBrokenRule(BusinessRule brokenRule)
        {
            _brokenRules.Add(brokenRule);
        }

        #endregion

        #region Methods
        /// <summary>
        /// برابر بودن دو موجودیت
        /// </summary>
        /// <param name="entity">موجودیت</param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            return entity != null
                && entity is EntityBaseComplexID
                && this == (EntityBaseComplexID)entity;
        }

        /// <summary>
        /// عملگر تساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator ==(EntityBaseComplexID entity1, EntityBaseComplexID entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.ID1.ToString() == entity2.ID1.ToString() && entity1.ID2.ToString() == entity2.ID2.ToString())
                return true;

            return false;
        }

        /// <summary>
        /// عملگر نامساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator !=(EntityBaseComplexID entity1,
            EntityBaseComplexID entity2)
        {
            return !(entity1 == entity2);
        }
#endregion
    }
}
