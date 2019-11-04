using System;
using Infrastructure.Domain;
using Model.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees
{
    public class ToDoResult : EntityBase, IAggregateRoot
    {
        public virtual Employee ReferedEmployee { get; set; }
        public virtual ToDo ToDo { get; set; }
        public virtual string ToDoResultDescription { get; set; }
        public virtual string RemindeTime { get; set; }
        public virtual bool SecondaryClosed { get; set; }
        public virtual bool Remindable { get; set; }
        public virtual string Attachment { get; set; }
        public virtual string AttachmentType { get; set; }
        public virtual bool IsMine { get {
            if (ReferedEmployee.ID == CreateEmployee.ID)
                return true;
            else
                return false;
        } }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
