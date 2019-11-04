using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportTicketWaitingResponseBusinessRule
    {
        public static readonly BusinessRule SupportRequired = new BusinessRule("Support", "پشتیبانی باید وارد شود");
        public static readonly BusinessRule SendTicketDateRequired = new BusinessRule("SendTicketDate", "تاریخ ارسال تیکت باید وارد شود");
        public static readonly BusinessRule TicketNumberRequired = new BusinessRule("TicketNumber", "شماره تیکت باید وارد شود");
        public static readonly BusinessRule ResponsePossibilityDateRequired = new BusinessRule("ResponsePossibilityDate", "تاریخ احتمال باید وارد شود");
        public static readonly BusinessRule CommentRequired = new BusinessRule("Comment", "توضیحات باید وارد شود");
    }
}
