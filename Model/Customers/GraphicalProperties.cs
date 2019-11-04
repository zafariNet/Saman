#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Model.Customers
{
    [Serializable]
    public class GraphicalProperties
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool EnableDragging { get; set; }
    }
}
