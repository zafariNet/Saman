#region Usings
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;
#endregion

namespace Model.Customers
{
    public class Document : EntityBase, IAggregateRoot
    {
        /// <summary>
        ///  مربوط به مشتری
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// نام مدرک
        /// </summary>
        public virtual string DocumentName { get; set; }
        /// <summary>
        /// تصویر مدرک
        /// </summary>
        public virtual string Photo { get; set; }
        /// <summary>
        /// نوع عکس
        ///example: jpg, bmp, gif...
        /// </summary>
        public virtual string ImageType { get; set; }
        /// <summary>
        /// تاریخ دریافت مدرک
        /// </summary>
        public virtual string ReceiptDate { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        #region Validate

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Customer == null)
                base.AddBrokenRule(DocumentBusinessRules.CustomerRequired);
            //if (this.ReceiptDate == null)
            //    base.AddBrokenRule(DocumentBusinessRules.ReceiptDateRequired);
        }
        #endregion
    }
}
