using System.Collections.Generic;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class QuestionMapper
    {
        public static IEnumerable<QuestionView> ConvertToQuestionViews(
            this IEnumerable<Question> questions)
        {
            return Mapper.Map<IEnumerable<Question>,
                IEnumerable<QuestionView>>(questions);
        }

        public static QuestionView ConvertToQuestionView(this Question question)
        {
            return Mapper.Map<Question, QuestionView>(question);
        }
    }
}
