using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddNoteRequest
    {
        public Guid CustomerID { get; set; }
        public string NoteDescription { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }
}
