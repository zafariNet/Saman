using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class PermitView : BaseView
    {
        public PermissionView Permission { get; set; }
        public string PermitKey { get; set; }
        public bool Guaranteed { get; set; }
    }
}
