using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Customers
{
    public class AnswerView:BaseView
    {
        public virtual string AnswerText { get; set; }
        public string QuestionText { get; set; }
        public Guid QuestionID { get; set; }

    }
}
