using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.EmployeeCatalogService
{
    public class EditToDoResultRequest:AddToDoResulRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }
}
