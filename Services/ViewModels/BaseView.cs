#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Services.ViewModels
{
    public class BaseView
    {
        #region Base Properties

        [Key]
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }

        [Display(Name = "تاریخ ایجاد رکورد")]
        public string CreateDate { get; set; }

        public Guid CreateEmployeeID { get; set; }

        [Display(Name = "کارمند ایجاد کننده رکورد")]
        public string CreateEmployeeName { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string ModifiedDate { get; set; }

        [Display(Name = "کارمند ویرایش کننده")]
        public string ModifiedEmployeeName { get; set; }

        public int RowVersion { get; set; }

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
                && entity is BaseView
                && this == (BaseView)entity;
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
        public static bool operator ==(BaseView entity1, BaseView entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.ID.ToString() == entity2.ID.ToString())
                return true;

            return false;
        }

        /// <summary>
        /// عملگر نامساوی منطقی دو موجودیت
        /// </summary>
        /// <param name="entity1">موجودیت اول</param>
        /// <param name="entity2">موجودیت دوم</param>
        /// <returns>True/False</returns>
        public static bool operator !=(BaseView entity1,
            BaseView entity2)
        {
            return !(entity1 == entity2);
        }
        #endregion

    }
}
