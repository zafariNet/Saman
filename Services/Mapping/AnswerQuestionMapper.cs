using System.Collections.Generic;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class AnswerQuestionMapper
    {
        public static IEnumerable<QuestionAnswerView> ConvertToQuestionAnswerViews(
            this IEnumerable<QuestionAnswer> answers)
        {
            return Mapper.Map<IEnumerable<QuestionAnswer>,
                IEnumerable<QuestionAnswerView>>(answers);
        }

        public static QuestionAnswerView ConvertToQuestionAnswerView(this QuestionAnswer answer)
        {
            return Mapper.Map<QuestionAnswer, QuestionAnswerView>(answer);
        }
    }
}
