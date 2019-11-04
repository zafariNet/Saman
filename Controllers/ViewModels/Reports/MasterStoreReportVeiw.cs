using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Model.Store;

namespace Controllers.ViewModels.Reports
{
    class MasterStoreReportVeiw
    {
        #region Customer Properties 
        /// <summary>
        /// شماره تلفن ADSL
        /// </summary>
        public virtual string ADSLPhone { get; set; }
        #endregion

        #region Product Properties
        /// <summary>
        /// فروش کالا
        /// </summary>
        public virtual IEnumerable<Model.Sales.ProductSaleDetail> ProductSaleDetails { get; protected set; }
        /// <summary>
        /// نام دسته بندی کالا
        /// </summary>
        public virtual ProductCategory ProductCategory { get; set; }
        /// <summary>
        /// تعداد موجودی انبار اصلی بدون در نظر گرفتن موجودی انبارهای فرعی
        /// </summary>
        public virtual int UnitsInStock { get; set; }
        /// <summary>
        /// تعداد ورود یا خروج
        /// </summary>
        public virtual int UnitsIO { get; set; }
        /// <summary>
        /// نام طبقه کالا
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public virtual string Note { get; set; }
        #endregion

        #region Store Properties
        /// <summary>
        /// کارمند - مشخص می کند که انبار متعلق به کدام کارشناس است
        /// </summary>
        public virtual Employee OwnerEmployee { get; set; }
        #endregion
        #region ProductLog Properties
        /// <summary>
        /// تاریخ ورود به انبار یا خروج از انبار
        /// </summary>
        public virtual string LogDate { get; set; }

        #endregion

    }
}
