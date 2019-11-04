using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class DocumentMap : ClassMapping<Document>
    {
        public DocumentMap()
        {
            Table("Cus.Document");

            // Base Properties
            Id(x => x.ID, c => c.Column("DocumentID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);

            // Document Properties
            ManyToOne(x => x.Customer, c => c.Column("CustomerID"));
            Property(x => x.DocumentName, c => c.Length(25));
            Property(x => x.Photo, c => c.Length(256));
            Property(x => x.ImageType, c => c.Length(10));
            Property(x => x.ReceiptDate, c => c.Length(10));
            Property(x => x.Note);
        }
    }
}
