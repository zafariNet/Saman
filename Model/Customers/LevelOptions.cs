#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Model.Customers
{
    [Serializable]
    public class LevelOptions
    {
        public bool CanSale { get; set; }

        public bool CanChangeNetwork { get; set; }

        public bool CanPersenceSupport { get; set; }

        public bool CanAddProblem { get; set; }

        public bool CanDocumentsOperation { get; set; }
    }
}
