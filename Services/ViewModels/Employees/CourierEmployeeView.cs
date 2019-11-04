using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Sales;

namespace Services.ViewModels.Employees
{
    public class CourierEmployeeView:BaseView
    {


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public IEnumerable<CourierView> Couriers { get; set; }

    }
}
