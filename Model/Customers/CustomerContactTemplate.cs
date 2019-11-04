using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;
using Model.Employees;

namespace Model.Customers
{
    /// <summary>
    /// موجودیت ثبت تماس
    /// </summary>
    public class CustomerContactTemplate : EntityBase, IAggregateRoot
    {
        #region Properties

        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual Group Group { get; set; }

        #endregion

        #region Validation

        protected override void Validate()
        {
            if (string.IsNullOrEmpty("Title"))
                AddBrokenRule(CustomerContactTemplateBusinessRule.CustomerContactTemplateTitleRequired);
        }

        #endregion
    }
}
