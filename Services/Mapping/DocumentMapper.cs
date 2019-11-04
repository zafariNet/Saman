using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class DocumentMapper
    {
        public static IEnumerable<DocumentView> ConvertToDocumentViews(
            this IEnumerable<Document> documents)
        {
            return Mapper.Map<IEnumerable<Document>,
                IEnumerable<DocumentView>>(documents);
        }

        public static DocumentView ConvertToDocumentView(this Document document)
        {
            return Mapper.Map<Document, DocumentView>(document);
        }
    }
}
