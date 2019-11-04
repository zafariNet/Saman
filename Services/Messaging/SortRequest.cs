using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging
{
    public class SortRequest
    {
        // Field Name For sort
        public string FieldName { get; set; }

        // ASC, DESC
        public bool Asc { get; set; }
    }
}
