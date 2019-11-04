using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Store;

namespace Model.Customers
{
    public class NetworkCenterPriority:IAggregateRoot
    {

        public virtual Network Network { get; set; }
        public virtual Center Center { get; set; }
        public virtual int SalePriority { get; set; }


    }
}
