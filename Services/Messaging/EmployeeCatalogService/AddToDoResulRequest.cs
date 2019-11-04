using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddToDoResulRequest
    {
        public  Guid ReferedEmployeeID { get; set; }
        public  Guid ToDoID { get; set; }
        public  string ToDoResultDescription { get; set; }
        public  string RemindeTime { get; set; }
        public  bool SecondaryClosed { get; set; }
        public  bool Remindable { get; set; }
    }
}
