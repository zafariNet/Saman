﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.Leadcatalogservice
{
    public class EditLeadResultTemplateRequest:AddLeadResultTemplateRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }
}
