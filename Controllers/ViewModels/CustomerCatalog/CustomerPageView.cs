using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.ViewModels.Customers;
using Services.ViewModels.Sales;
using Services.ViewModels.Support;
using Services.ViewModels.Fiscals;

namespace Controllers.ViewModels.CustomerCatalog
{
    public class CustomerPageView : BasePageView
    {
        /// <summary>
        /// اطلاعات اصلی مشتری
        /// </summary>
        public CustomerView CustomerView { get; set; }
        /// <summary>
        /// اسناد و مدارک
        /// </summary>
        public IEnumerable<DocumentView> DocumentViews { get; set; }
        /// <summary>
        /// تاریخچه صورتحساب مشتری
        /// </summary>
        public IEnumerable<SaleView> SaleViews { get; set; }
        /// <summary>
        /// تاریخچه امور مالی مشتری
        /// </summary>
        public IEnumerable<FiscalView> FiscalViews { get; set; }
        public FiscalView FiscalView { get; set; }
        /// <summary>
        /// تاریخچه پشتیبانی حضوری در محل مشتری
        /// </summary>
        public IEnumerable<PersenceSupportView> PersenceSupportViews { get; set; }
        /// <summary>
        /// تاریخچه مشکلات مشتری
        /// </summary>
        public IEnumerable<ProblemView> ProblemViews { get; set; }
        /// <summary>
        /// ایمیل های ارسال شده به مشتری
        /// </summary>
        public IEnumerable<EmailView> EmailViews { get; set; }
        /// <summary>
        /// پیام کوتاههای ارسال شده به مشتری
        /// </summary>
        public IEnumerable<SmsView> SmsViews { get; set; }
        /// <summary>
        /// همه یادداشت های مشتری
        /// </summary>
        public IEnumerable<NoteView> AllNoteViews { get; set; }
        /// <summary>
        /// یادداشت های مربوط به این مرحله مشتری
        /// </summary>
        public IEnumerable<NoteView> ThisLevelNoteViews { get; set; }
    }
}
