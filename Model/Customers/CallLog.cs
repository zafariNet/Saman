using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    public enum CallType
    {
        Incomming,
        Outgoing
    }
    public class CallLog : EntityBase, IAggregateRoot
    {

        #region Properties

        public virtual Customer Customer { get; set; }
        public virtual string LocalPhone { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual CustomerContactTemplate CustomerContactTemplate { get; set; }
        public virtual string Description { get; set; }
        public virtual CallType CallType { get; set; }
        #endregion

        #region Validation

        protected override void Validate()
        {
            if(Customer ==null)
                AddBrokenRule(CallLogBusinessRule.CustomerRequired);
            if(string.IsNullOrEmpty(LocalPhone))
                AddBrokenRule(CallLogBusinessRule.LocalPhoneRequired);
        }

        #endregion
    }
}
