using System;
using System.Collections.Generic;
namespace Controllers.ViewModels.EmployeeCatalog
{
    public class EmployeeLoginView
    {
        public Guid EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public bool IsLogedIn { get; set; }

        public string Picture { get; set; }

        public Dictionary<string, bool> Permits { get; set; }

        public string ServerDate { get { return DateTime.Now.Date.ToShortDateString(); } }
    }
}
