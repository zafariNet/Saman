using System;


namespace Services.Messaging.Leadcatalogservice
{
    public class AddLeadResultTemplateRequest
    {
        public string LeadResulTitle { get; set; }
        public string Description { get; set; }
        public Guid GroupID { get; set; }
    }
}
