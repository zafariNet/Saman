using System;

namespace Services.ViewModels.Customers
{
    public class CallLogView:BaseView
    {
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string LocalPhone { get; set; }
        public string Description { get; set; }
        public string CallType { get; set; }
        public string CustomerContactTemplateTitle { get; set; }
        public string CustomerContactTemplateDescription { get; set; }
    }
}
