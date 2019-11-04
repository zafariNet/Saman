using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sales
{
    public class SuctionModeCost
    {
        public Guid SuctionModeDetailID { get; set; }
                    
        public string CenterName { get; set; }
        public int CustomerPerCenter { get; set; }
        public string SuctionModeDetailName { get; set; }
        
        public long Amount { get; set; }
        public int UserCountPerSuctionModeDetal { get; set; }

        public string SuctionModeName { get; set; }
    }
}
