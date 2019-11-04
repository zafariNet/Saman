using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class DocumentStatusMapper
    {
        public static IEnumerable<DocumentStatusView> ConvertToDocumentStatusViews(
            this IEnumerable<DocumentStatus> documentStatuss)
        {
            return Mapper.Map<IEnumerable<DocumentStatus>,
                IEnumerable<DocumentStatusView>>(documentStatuss);
        }

        public static DocumentStatusView ConvertToDocumentStatusView(this DocumentStatus documentStatus)
        {
            return Mapper.Map<DocumentStatus, DocumentStatusView>(documentStatus);
        }
    }
}
