using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.Customers;
using Services.ViewModels.Fiscals;
using Model.Employees;

namespace Services.ViewModels.Employees
{
    public class    SimpleEmployeeView:BaseView
    {
        //Temporary
        
        public Guid EmployeeID { get; set; }
        //public string Name { get; set; }
        //public Guid GroupID { get; set; }
        //public string GroupName { get; set; }
        //public string ParentEmployeeName { get; set; }


        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام کارمند")]
        public string Name { get; set; }

        public Guid GroupID { get; set; }

        [Display(Name = "نام گروه")]
        public string GroupName { get; set; }

        public Guid ParentEmployeeID { get; set; }

        [Display(Name = "کارمند بالادستی")]
        public string ParentEmployeeName { get; set; }
        public bool Discontinued { get; set; }
       // public IList<EmployeeView> AllChildEmployees { get; set; }


    }
}
