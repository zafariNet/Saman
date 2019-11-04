using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddConditionRequestOld
    {
        public string ConditionTitle { get; set; }
        public string QueryText { get; set; }
        //public string PropertyName { get; set; }
        //public string Value { get; set; }
        //public short CriteriaOperator { get; set; }
        public string ErrorText { get; set; }
        public bool nHibernate { get; set; }
        public Guid CreateEmployeeID { get; set; }
    }

    public class AddConditionRequest
    {
        public string ConditionTitle { get; set; }
        public string QueryText { get; set; }
        //public string PropertyName { get; set; }
        //public string Value { get; set; }
        //public short CriteriaOperator { get; set; }
        public string ErrorText { get; set; }
        public bool nHibernate { get; set; }
    }
}
