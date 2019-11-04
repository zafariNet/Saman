using Infrastructure.Domain;
using Model.Base;
using Model.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees
{
    public enum PriorityType
    {
        low=1,
        mediom=2,
        High=3
    }
    public class ToDo : EntityBase, IAggregateRoot
    {
        public virtual string ToDoTitle { get; set; }
        public virtual string ToDoDescription { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual string Attachment { get; set; }
        public virtual string AttachmentType { get; set; }
        public virtual bool IsGrouped { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string EndDate { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string EndTime { get; set; }
        public virtual PriorityType PriorityType { get; set; }
        public virtual bool PrimaryClosed { get; set; }
        public virtual bool IsMine { get; set; }
        public virtual IEnumerable<ToDoResult> ToDoResults { get; set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}