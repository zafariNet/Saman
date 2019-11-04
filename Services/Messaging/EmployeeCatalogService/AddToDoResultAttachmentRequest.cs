using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class AddToDoResultAttachmentRequest
    {
        public Guid ToDoResultID { get; set; }
        public string AttachmentName { get; set; }
        public string Attachment { get; set; }
        public string AttachmentType { get; set; }
    }
}
