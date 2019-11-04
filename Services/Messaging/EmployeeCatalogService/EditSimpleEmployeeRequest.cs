using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;

namespace Services.Messaging.EmployeeCatalogService
{
    //Added By Zafari
    // از این مدل برای ویرایش یک کارمند برای غیرفعال کردن کارمند استفاده میشود
    public class EditSimpleEmployeeRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }

        public Guid ModifiedEmployeeID { get; set; }
        public bool Discontinued { get; set; }
    }
}
