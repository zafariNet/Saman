using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddCustomerRequest
    {
        public Guid CenterID { get; set; }
        public Guid AgencyID { get; set; }
        public Guid NetworkID { get; set; }
        public Guid SuctionModeID { get; set; }
        public Guid SuctionModeDetailID { get; set; }
        public Guid DocumentStatusID { get; set; }
        public Guid BuyPossibilityID { get; set; }
        public Guid FollowStatusID { get; set; }
        public Guid levelTypeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string Job { get; set; }
        public string Phone { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string SLastName { get; set; }
        public string SFirstName { get; set; }
        public string ADSLPhone { get; set; }
        public string LegalType { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public bool Locked { get; set; }
        public Int64 Balance { get; set; }
        public long CanDeliverCost { get; set; }
        public Guid LockEmployeeID { get; set; }
        public string LockNote { get; set; }
        public bool SentToPap { get; set; }
        public bool Discontinued { get; set; }
        public Guid CreateEmployeeID { get; set; }
        public string Mobile { get; set; }
        public Guid LevelID { get; set; }
    }
}
