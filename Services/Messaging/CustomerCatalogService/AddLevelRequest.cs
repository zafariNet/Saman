using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddLevelRequest
    {
        public Guid LevelTypeID { get; set; }
        public string LevelTitle { get; set; }
        public string LevelNikname { get; set; }
        public bool OnEnterSendSMS { get; set; }
        public bool OnEnterSendEmail { get; set; }
        public string OnEnter { get; set; }
        public string OnExit { get; set; }
        public string EmailText { get; set; }
        public string SMSText { get; set; }
        public bool IsFirstLevel { get; set; }
        public string Options { get; set; }
        public bool Discontinued { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public bool HasRequireNetwok { get; set; }
        public Guid LevelStaffId { get; set; }
    }
}
