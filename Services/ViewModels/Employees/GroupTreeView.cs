using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class GroupTreeView
    {
        public string GroupName { get; set; }
        public Guid GroupID { get; set; }
        public Guid ParentGroupID { get; set; }
        public Guid GroupStaffID { get; set; }
        public string TotalEmployees { get; set; }
        public int RowVersion { get; set; }
    } 
}
