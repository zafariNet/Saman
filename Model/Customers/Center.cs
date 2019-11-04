using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;
using Model.Customers.Validations;

namespace Model.Customers
{
    /// <summary>
    /// موجودیت مرکز مخابراتی
    /// </summary>
    public class Center : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// نام مرکز مخابراتی
        /// </summary>
        public virtual string CenterName { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        ///  وضعیت مرکز
        /// </summary>
        public virtual string Status
        {
            get
            {
                string status = "";

                //  اگر حداقل یک شبکه تحت پوشش باشد
                if (SupportNetworksCount > 0)
                    status = "تحت پوشش";
                // در غیر اینصورت اگر حداقل یک شبکه عدم امکان موقت باشد
                else if (AdameEmkanNetworksCount > 0)
                    status = "عدم امکان";
                // اگر تحت پوشش نبود عدم امکان هم نبود پس عدم پوشش است. نامشخص هم عدم پوشش است
                else status = "عدم پوشش";

                return status;
            }
        }

        public virtual string StatusKey
        {
            get
            {
                string status = "";

                //  اگر حداقل یک شبکه تحت پوشش باشد
                if (SupportNetworksCount > 0)
                    status = "Support";
                // در غیر اینصورت اگر حداقل یک شبکه عدم امکان موقت باشد
                else if (AdameEmkanNetworksCount > 0)
                    status = "AdameEmkan";
                // اگر تحت پوشش نبود عدم امکان هم نبود پس عدم پوشش است. نامشخص هم عدم پوشش است
                else status = "NotSupport";

                return status;
            }
        }

        #region Private Status Methods
        // تعداد شبکه های تحت پوشش
        private int SupportNetworksCount
        {
            get
            {
                return NetworkCenters.Where(w => w.Status == NetworkCenterStatus.Support).Count();
            }
        }

        // تعداد شبکه های عدم پوشش
        private int NotSupportNetworksCount
        {
            get
            {
                return NetworkCenters.Where(w => w.Status == NetworkCenterStatus.NotSupport).Count();
            }
        }

        // تعداد شبکه های عدم امکان موقت
        private int AdameEmkanNetworksCount
        {
            get
            {
                return NetworkCenters.Where(w => w.Status == NetworkCenterStatus.AdameEmkan).Count();
            }
        }

        // تعداد شبکه های نامشخص
        private int NotDefinedNetworksCount
        {
            get
            {
                return NetworkCenters.Where(w => w.Status == NetworkCenterStatus.NotDefined).Count();
            }
        }     
        #endregion    

        public virtual IEnumerable<NetworkCenter> NetworkCenters { get; set; } 

        /// <summary>
        /// تعداد مشتریان
        /// </summary>
        //public virtual int CustomerCount { get; protected set; }

        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        protected override void Validate()
        {
            if (this.CenterName == null)
                base.AddBrokenRule(CenterBusinessRules.CenterNameRequired);
        }

    }
}
