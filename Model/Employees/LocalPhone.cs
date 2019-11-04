#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees.Validations;

#endregion

namespace Model.Employees
{
    /// <summary>
    /// تلفنهای داخلی
    /// </summary>
    public class LocalPhone : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// شماره تلفن داخلی
        /// </summary>
        public virtual string LocalPhoneNumber { get; set; }
        /// <summary>
        /// کارمند صاحب تلفن
        /// </summary>
        public virtual Employee OwnerEmployee { get; set; }
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            //if (this.LocalPhoneNumber == null)
            //    base.AddBrokenRule(GroupBusinessRules.GroupNameRequired);
            //if (this.Permissions == null)
            //    base.AddBrokenRule(GroupBusinessRules.PermissionsRequired);
        }
    }
}
