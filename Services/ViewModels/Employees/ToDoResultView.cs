using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class ToDoResultView:BaseView
    {
        public Guid ReferedEmployeeID { get; set; }
        public string ReferedEmployeeName { get; set; }
        public string ToDoResultDescription { get; set; }
        public  string Attachment { get; set; }
        public  string AttachmentType { get; set; }
        public string RemindeTime { get; set; }
        public bool SecondaryClosed { get; set; }
        public bool Remindable { get; set; }
        public bool IsMine { get; set; }
        
    }
}
