using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging
{
    public class MainMenusGetRequest
    {
        public string ParentMenuName { get; set; }

        public Guid EmployeeID { get; set; }
    }
}
