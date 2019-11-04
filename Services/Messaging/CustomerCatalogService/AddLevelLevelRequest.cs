using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Messaging.CustomerCatalogService
{
    public class AddLevelLevelRequest
    {
        public Guid LevelID { get; set; }

        public Guid NextLevelID { get; set; }
    }
}
