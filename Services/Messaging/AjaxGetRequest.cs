using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging
{
    public class AjaxGetRequest
    {
        public Guid ID { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
