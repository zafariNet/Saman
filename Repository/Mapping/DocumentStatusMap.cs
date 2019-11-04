using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class DocumentStatusMap : ClassMapping<DocumentStatus>
    {
        public DocumentStatusMap()
        {
            Table("Cus.DocumentStatus");

            // Base Properties
            Id(x => x.ID, c => c.Column("DocumentStatusID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // DocumentStatus Properties
            Property(x => x.DocumentStatusName, c => c.Length(100));
            Property(x => x.DefaultStatus);
            Property(x => x.CompleteStatus);
            Property(x => x.SortOrder);

            //Bags
            //Bag(x => x.Customers,
            //collectionMapping =>
            //{
            //    collectionMapping.Table("Cus.Customer");
            //    //collectionMapping.Access(typeof(long));
            //    collectionMapping.Cascade(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
            //    collectionMapping.Key(k => k.Column("DocumentStatusID"));
            //    collectionMapping.Lazy(NHibernate.Mapping.ByCode.CollectionLazy.Lazy);
            //},
            //   mapping => mapping.OneToMany());
        }
    }
}
