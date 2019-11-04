using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Model.Base;
using Infrastructure.Domain;
using System.IO.Compression;

namespace Model.Employees
{
    public class Permit : EntityBase, IAggregateRoot
    {
        public Permit()
        {

        }

        public Permit(Permission permission)
        {
            Permission = permission;
            ID = Guid.NewGuid();
        }

        public virtual Permission Permission { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Group Group { get; set; }

        public virtual string PermitKey
        {
            get
            {
                return Permission.Key;
            }

            set
            {
                Permission.Key = value;
            }
        }

        public virtual bool Guaranteed { get; set; }

        protected override void Validate()
        {
            
        }
    }
}

