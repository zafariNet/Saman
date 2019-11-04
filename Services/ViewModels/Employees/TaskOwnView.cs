using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;

namespace Services.ViewModels.Employees
{
    public class TaskOwnView:BaseView
    {

        public string ToDoTitle { get; set; }

        public string ToDoDescription { get; set; }

        public string ReferedEmployeeName { get; set; }

        public  string  CustomerName { get; set; }

        public bool IsMaster { get; set; }

        public string ADSLPhone { get; set; }

        public bool PrimaryClosed { get; set; }

        public string PrimaryClosedDate { get; set; }
        public bool SecondaryClosed { get; set; }

        public string SecondaryClosedDate { get; set; }

        public string ToDoResultDescription { get; set; }

        public string PrimaryFile { get; set; }


        public string SecondaryFile { get; set; }

        public IEnumerable<TaskOwnView> ToDoResults { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Reminder { get; set; }
        public  string RemindTime { get; set; }
        public bool SendSms { get; set; }
        public bool Completed { get; set; }
        public bool CanEditDetail { get; set; }
        public bool CanEditMaster { get; set; }

    }
}
