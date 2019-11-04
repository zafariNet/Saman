using System;

namespace Services.ViewModels.Leads
{
    public class LeadResultTemplateView:BaseView
    {
        public string LeadResulTitle { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public Guid GroupID { get; set; }
    }
}
