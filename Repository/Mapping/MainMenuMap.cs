#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using Model;

#endregion

namespace Repository.Mapping
{
    public class MainMenuMap : ClassMapping<MainMenu>
    {
        public MainMenuMap()
        {
            Table("dbo.MainMenu");

            Id(x => x.ID, c => c.Column("ID"));
            Property(x => x.ParentMenuName);
            Property(x => x.SubmenuName);
            Property(x => x.SubmenuUrl);
            Property(x => x.xType);
            Property(x => x.Icon);
            Property(x => x.SortOrder);
            Property(x => x.PermissionKey);
            Property(x => x.Show);
            Property(x=>x.PreLoad);
        }
    }
}
