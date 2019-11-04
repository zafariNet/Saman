using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class AnswerMapper
    {
        public static IEnumerable<AnswerView> ConvertToAnswerViews(
            this IEnumerable<Answer> answers)
        {
            return Mapper.Map<IEnumerable<Answer>,
                IEnumerable<AnswerView>>(answers);
        }

        public static AnswerView ConvertToAnswerView(this Answer answer)
        {
            return Mapper.Map<Answer, AnswerView>(answer);
        }
    }
}
