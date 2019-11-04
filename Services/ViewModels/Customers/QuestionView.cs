using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Customers
{
    public class QuestionView:BaseView
    {
        public string QuestionText { get; set; }
        public string LevelTitle { get; set; }
        public Guid LevelID { get; set; }
        public IEnumerable<AnswerView> AnswerViews { get; set; }
    }
}
