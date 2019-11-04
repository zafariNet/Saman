using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Support;
using Services.ViewModels.Support;
using AutoMapper;

namespace Services.Mapping
{
    public static class ProblemMapper
    {
        public static IEnumerable<ProblemView> ConvertToProblemViews(
            this IEnumerable<Problem> problems)
        {
            return Mapper.Map<IEnumerable<Problem>,
                IEnumerable<ProblemView>>(problems);
        }

        public static ProblemView ConvertToProblemView(this Problem problem)
        {
            return Mapper.Map<Problem, ProblemView>(problem);
        }
    }
}
