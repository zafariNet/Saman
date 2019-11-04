using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repository.Mapping
{
    public class MessageTemplateMap:ClassMapping<MessageTemplate>
    {
        public MessageTemplateMap()
        {
            Table("Emp.MessageTemplate");

            // Base Properties
            Id(x => x.ID, c => c.Column("MessageTemplateID"));
            Property(x => x.CreateDate, m => m.Length(19));
            Property(x => x.ModifiedDate, m => m.Length(19));
            ManyToOne(x => x.CreateEmployee, c => c.Column("EmployeeID"));
            ManyToOne(x => x.ModifiedEmployee, c => c.Column("ModifiedEmployeeID"));
            Property(x => x.RowVersion);
            Property(x=>x.MessageType);
            Property(x=>x.MessageTemplateName);
            Property(x=>x.MessageEmailTemplateText);
            Property(x=>x.MessageSmsTemplateText);
        }
    }
}
