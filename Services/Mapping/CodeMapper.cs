using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class CodeMapper
    {
        public static IEnumerable<CodeView> ConvertToCodeViews(
            this IEnumerable<Code> codes)
        {
            return Mapper.Map<IEnumerable<Code>,
                IEnumerable<CodeView>>(codes);
        }

        public static CodeView ConvertToCodeView(this Code code)
        {
            return Mapper.Map<Code, CodeView>(code);
        }
    }
}
