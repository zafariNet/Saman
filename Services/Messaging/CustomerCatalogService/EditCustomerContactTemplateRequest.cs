using System;
namespace Services.Messaging.CustomerCatalogService
{
    public class EditCustomerContactTemplateRequest:AddCustomerContactTemplateRequest
    {
        public Guid ID { get; set; }
        public int RowVersion { get; set; }
    }
}
