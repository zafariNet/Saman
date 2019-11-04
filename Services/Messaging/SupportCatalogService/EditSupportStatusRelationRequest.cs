using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.SupportCatalogService
{
    public class EditSupportStatusRelationRequest:AddSupportStatusRelationRequest
    {
        public Guid ID { get; set; }
    }
}
