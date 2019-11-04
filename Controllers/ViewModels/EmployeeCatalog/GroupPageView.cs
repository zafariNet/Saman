using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Employees;

namespace Controllers.ViewModels.EmployeeCatalog
{
    public class GroupPageView : BasePageView
    {
        public GroupView GroupView { get; set; }

        public IEnumerable<GroupView> GroupViews { get; set; }
        /// <summary>
        /// کارمندانی که عضو این گروه هستند 
        /// </summary>
        public IList<EmployeeView> Employees { get; set; }

    }
}
