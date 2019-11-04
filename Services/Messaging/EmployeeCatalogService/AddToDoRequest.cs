using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddToDoRequest
    {
        public  string ToDoTitle { get; set; }
        public  string ToDoDescription { get; set; }
        public  Guid CustomerID { get; set; }
        public  string StartDate { get; set; }
        public  string EndDate { get; set; }
        public  string StartTime { get; set; }
        public  string EndTime { get; set; }
        public  int PriorityType { get; set; }
        public  bool PrimaryClosed { get; set; }
        public bool Remindable { get; set; }
        public string RemindeTime { get; set; }
        public IEnumerable<Guid> EmployeeIDs { get; set; }
        public IEnumerable<Guid> GroupIDs { get; set; }
        public bool IsGrouped { get; set; }
    }
}
