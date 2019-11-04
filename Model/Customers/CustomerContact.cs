
using Infrastructure.Domain;
using Model.Base;

namespace Model.Customers
{
    /// <summary>
    /// موجودیت نتیجه تماس
    /// </summary>
    public class CustomerContact : EntityBase, IAggregateRoot
    {
        #region Properties



        #endregion

        #region Validation

        protected override void Validate()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
