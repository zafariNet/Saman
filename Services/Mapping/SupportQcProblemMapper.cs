using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportQcProblemMapper
    {
        public static IEnumerable<SupportQcProblemView> ConvertToSupportQcProblemViews(
            this IEnumerable<SupportQcProblem> supportQcProblemes)
        {
            return Mapper.Map<IEnumerable<SupportQcProblem>,
                IEnumerable<SupportQcProblemView>>(supportQcProblemes);
        }

        public static SupportQcProblemView ConvertToSupportQcProblemView(
            this SupportQcProblem supportQcProblem)
        {
            return Mapper.Map<SupportQcProblem, SupportQcProblemView>(supportQcProblem);
        }
    }
}
