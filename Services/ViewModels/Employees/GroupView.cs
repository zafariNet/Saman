using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Model.Employees;

namespace Services.ViewModels.Employees
{
    public class GroupView : BaseView
    {
        [Display(Name = "نام گروه")]
        public string GroupName { get; set; }

        /// <summary>
        /// شناسه گروه بالادستی
        /// </summary>
        public Guid ParentGroupID { get; set; }
        /// <summary>
        /// شناسه مسئول گروه
        /// </summary>
        public Guid? GroupStaffID { get; set; }
        /// <summary>
        /// نام مسئول گروه
        /// </summary>
        public string GroupStaffName { get; set; }
        /// <summary>
        /// گروه بالادستی
        /// </summary>
        public string ParentGroupName { get; set; }

        [Display(Name = "تعداد کارمندان عضو گروه")]
        public string EmployeeCount { get; set; }

        [Display(Name = "دسترسیها")]
        public IEnumerable<Permit> Permissions { get; set; }
        

        //IEnumerables

        //[Display(Name = "کارمندان عضو این گروه")]
        //public IEnumerable<EmployeeView> Employees { get; set; }
    }
}
