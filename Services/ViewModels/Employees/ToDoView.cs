using Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Employees
{
    public class ToDoView:BaseView
    {
        public string ToDoTitle { get; set; }
        public string ToDoDescription { get; set; }
        public Guid CustomerID { get; set; }
        public string ADSLPhone { get; set; }
        public  string Attachment { get; set; }
        public  string AttachmentType { get; set; }
        public string CustomerName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsMine { get; set; }
        public PriorityType PriorityType { get; set; }
        public bool PrimaryClosed { get; set; }
        public IEnumerable<ToDoResultView> ToDoResults { get; set; }

        public IEnumerable<Guid> ToDoResultsID {
            get { return ToDoResults.Select(x => x.ID); }
        }
    }
}
