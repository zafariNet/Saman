using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddTaskRequest
    {
        public string ToDoTitle { get; set; }
        public string ToDoDescription { get; set; }
        public IEnumerable<Guid?> EmployeeIDs { get; set; }
        public IEnumerable<Guid?> GroupIDs { get; set; }
        public Guid? CustomerID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Reminder { get; set; }
        public string RemindTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
        public bool SendSms { get; set; }
    }
}
