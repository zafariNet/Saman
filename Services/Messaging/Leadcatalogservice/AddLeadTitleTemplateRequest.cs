using System;

namespace Services.Messaging.Leadcatalogservice
{
    public class AddLeadTitleTemplateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid GroupID { get; set; }
    }
}
