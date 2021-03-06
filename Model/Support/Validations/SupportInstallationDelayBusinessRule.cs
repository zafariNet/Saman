﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Support.Validations
{
    public class SupportInstallationDelayBusinessRule
    {
        public static readonly BusinessRule SupportRequired = new BusinessRule("Support", "پشتیبانی باید وارد شود");
        public static readonly BusinessRule InstallDateRequired = new BusinessRule("DispatchDate", "تاریخ نصب باید وارد شود");
        public static readonly BusinessRule NextCallDateRequired = new BusinessRule("NextCallDate", "تاریخ تماس بعدی باید وارد شود");
        public static readonly BusinessRule CommentRequired = new BusinessRule("Comment", "توضیحات باید وارد شود");
    }
}
