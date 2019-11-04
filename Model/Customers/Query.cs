using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    /// <summary>
    /// این موجودیت نماهای ایجاد شده توسط مشتری را به همراه کوئری و پارامترهای آن نگهداری می کند
    /// </summary>
    public class Query : EntityBase, IAggregateRoot
    {
        public Query()
        {
            Level=new Level();
        }

        /// <summary>
        /// عنوان نما
        /// </summary>
        public virtual string Title { get; set; }

        public virtual bool Counting { get; set; }

        public virtual bool AllCustomer { get; set; }
        /// <summary>
        /// متن کوئری نما
        /// </summary>
        public virtual string QueryText { get; set; }
        /// <summary>
        /// xType
        /// </summary>
        public virtual string xType { get; set; }
        /// <summary>
        /// liste parametrhaye nema. mesal: :CustomerID, :EmployeeID, ...
        /// </summary>
        public virtual string PrmDefinition { get; set; }
        /// <summary>
        /// meghdare parametrhaye nema. Mesal: '12', '34', '45'
        /// </summary>
        public virtual string PrmValues { get; set; }
        /// <summary>
        /// ستون های قابل نمایش در این نما
        /// </summary>
        public virtual string Columns { get; set; }

        /// <summary>
        /// بازیابی اولیه
        /// </summary>
        public virtual bool PreLoad { get; set; }

        public virtual Level Level { get; set; }

        /// <summary>
        ///  پرسمان مشتری ها
        /// </summary>
        public virtual IEnumerable<QueryEmployee> QueryEmployees { get; protected set; }

        public virtual int CustomerCount { get; set; }
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.Title == null)
                base.AddBrokenRule(QueryBusinessRules.TitleRequired);
            if (this.QueryText == null)
                base.AddBrokenRule(QueryBusinessRules.QueryTextRequired);
        }  
    }
}
