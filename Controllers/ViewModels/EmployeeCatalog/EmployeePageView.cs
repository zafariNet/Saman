using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.EmployeeCatalog
{
    public class EmployeePageView : BasePageView
    {
        public EmployeeView EmployeView { get; set; }
        
        public IEnumerable<EmployeeView> EmployeViews { get; set; }
        /// <summary>
        /// نماهایی که توسط این کارمند قابل مشاهده است
        /// </summary>
        public IEnumerable<QueryView> QueryViews { get; set; }
        /// <summary>
        /// گروههای کاربری
        /// </summary>
        public IEnumerable<GroupView> GroupViews { get; set; }
    }
}
