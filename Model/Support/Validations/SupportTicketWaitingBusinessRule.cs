using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportTicketWaitingBusinessRule
    {
        public static readonly BusinessRule SupportRequired = new BusinessRule("Support", "پشتیبانی باید وارد شود");
        public static readonly BusinessRule DateOfPersenceDateRequired = new BusinessRule("DateOfPersenceDate", "تاریخ حضور کاشناس باید وارد شود");
        public static readonly BusinessRule InstallExpertRequired = new BusinessRule("InstallExpert", "کارشناس نصب باید وارد شود");
        public static readonly BusinessRule TicketSubjectRequired = new BusinessRule("TicketSubject", "موضوع تیکت باید وارد شود");
        public static readonly BusinessRule CommentRequired = new BusinessRule("Comment", "توضیحات باید وارد شود");
    }
}
