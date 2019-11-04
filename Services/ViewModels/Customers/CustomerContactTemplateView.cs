using System;

namespace Services.ViewModels.Customers
{
    public class CustomerContactTemplateView:BaseView
    {
        public string Title { get; set; }
        public string GroupName { get; set; }
        public Guid GroupID { get; set; }
    }
}
