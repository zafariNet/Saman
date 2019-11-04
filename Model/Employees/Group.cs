using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Employees.Validations;

namespace Model.Employees
{
    /// <summary>
    /// گروههای کاربری
    /// </summary>
    public class Group : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام گروه
        /// </summary>
        public virtual string GroupName { get; set; }
        /// <summary>
        /// گروه بالادستی
        /// </summary>
        public virtual Group ParentGroup { get; set; }
        /// <summary>
        /// گروه بالادستی
        /// </summary>
        public virtual Employee GroupStaff { get; set; }
        /// <summary>
        /// دسترسیها
        /// </summary>
        public virtual IEnumerable<Permit> Permissions { get; set; }

        /// <summary>
        /// کارمندان عضو این گروه
        /// </summary>
        public virtual IEnumerable<Employee> Employees { get; set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.GroupName == null)
                base.AddBrokenRule(GroupBusinessRules.GroupNameRequired);
            //if (this.Permissions == null)
            //    base.AddBrokenRule(GroupBusinessRules.PermissionsRequired);
        }
    }
}
