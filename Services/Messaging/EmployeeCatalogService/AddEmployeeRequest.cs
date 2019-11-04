using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddEmployeeRequest
    {
        public Guid ParentEmployeeID { get; set; }
        //Added By Zafari
        public Guid GroupID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        //public Permission Permissions { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string BirthDate { get; set; }
        public bool InstallExpert { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string HireDate { get; set; }
        public bool Discontinued { get; set; }
        public string Note { get; set; }
        public IEnumerable<Permit> Permissions { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public string Picture { get; set; }
    }
}
