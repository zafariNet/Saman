using Infrastructure.Domain;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Employees
{
    public class SimpleEmployee : EntityBase, IAggregateRoot
    {
        public virtual SimpleEmployee ParentEmployee { get; set; }
        public virtual IEnumerable<SimpleEmployee> ChildEmployees { get; protected set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// نام کارمند
        /// </summary>
        public virtual string Name { get { return FirstName + " " + LastName; } }
        /// <summary>
        /// کارشناس نصب
        /// </summary>
        public virtual bool InstallExpert { get; set; }
        public virtual Group Group { get; set; }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
