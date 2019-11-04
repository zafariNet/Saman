using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.Store;

namespace Services.ViewModels.Customers
{
    public class NetworkCenterView : BaseView
    {
        public NetworkCenterView()
        {
            //Status.Value = StatusInt;
            //Status.Text = StatusStr;
        }

        public Guid NetworkID { get; set; }

        public Guid CenterID { get; set; }

        [Display(Name = "مرکز مخابراتی")]
        public string CenterName { get; set; }

        [Display(Name = "شبکه")]
        public string NetworkName { get; set; }

        [Display(Name = "وضعیت")]
        public int StatusInt { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusStr { get; set; }

        [Display(Name = "وضعیت")]
        [UIHint("ClientStatus"), Required]
        public Status Status { get; set; }

        public bool CanSale { get; set; }
    }

    public class Status
    {
        public string Text { get; set; }

        public int Value { get; set; }
    }
}

