using System;

namespace Services.Messaging.Leadcatalogservice
{
    public class AddNegotiationRequest
    {
        public Guid?  ReferedEmployeeID { get; set; }
        public Guid  CustomerID { get; set; }
        public Guid LeadTitleTemplateID { get; set; }
        public string NegotiationDesciption { get; set; }
        public string NegotiationDate { get; set; }
        public string NegotiationTime { get; set; }
        public string RememberTime { get; set; }
        public bool? SendSms { get; set; }

    }
}
