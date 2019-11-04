using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class QuestionAnswerMapper
    {
        public static IEnumerable<QuestionAnswerView> ConvertToAgencyViews(
            this IEnumerable<QuestionAnswer> agencys)
        {
            return Mapper.Map<IEnumerable<QuestionAnswer>,
                IEnumerable<QuestionAnswerView>>(agencys);
        }

        public static QuestionAnswerView ConvertToAgencyView(this QuestionAnswer agency)
        {
            return Mapper.Map<QuestionAnswer, QuestionAnswerView>(agency);
        }
    }
}
