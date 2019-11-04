using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Leads
{
    public class NegotiationView:BaseView
    {
        public Guid ReferedEmployeeID  { get; set; }
        public string ReferedEmployeeName { get; set; }
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ADSLPhone { get; set; }
        public string LeadTitleTemplateTitle { get; set; }
        public Guid LeadTitleTemplateID { get; set; }
        public  string NegotiationDesciption { get; set; }
        public virtual string NegotiationDate { get; set; }
        public virtual string NegotiationTime { get; set; }
        public virtual string RememberDate { get; set; }
        public virtual string RememberTime { get; set; }
        public virtual bool SendSms { get; set; }
        public Guid  LeadResultTemplateID { get; set; }
        public string LeadResulTitle { get; set; }
        public string NegotiationResultDescription { get; set; }
        public string CloseDate { get; set; }
        public bool Closed { get; set; }
        public string NegotiationStatus { get; set; }
        public bool CanDelete{
            get
            {
                if (Closed)
                    return false;
                return true;
            }
        }

    }
}
