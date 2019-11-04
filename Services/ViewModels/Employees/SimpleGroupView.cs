using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class SimpleGroupView:BaseView
    {
        public string GroupName { get; set; }
        public List<SimpleEmployeeView> Employees { get; set; }
    }
}
