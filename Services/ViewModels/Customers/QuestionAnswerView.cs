using System;

namespace Services.ViewModels.Customers
{
    public class QuestionAnswerView:BaseView
    {
        public string AnswerText { get; set; }
        public Guid AnswerID { get; set; }

        public string QuestionText { get; set; }
        public Guid QuestionID { get; set; }
        public string LevelTitle { get; set; }
        public Guid LevelID { get; set; }
        public string CustomerName { get; set; }
        public string ADSLPhone { get; set; }
        public Guid CustomerID { get; set; }

    }
}
