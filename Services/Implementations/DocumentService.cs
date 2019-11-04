#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Querying;
using System.IO;
using Infrastructure.Domain;
using System.Collections;
#endregion

namespace Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        #region Declares

        private readonly IDocumentRepository _documentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region ctor

        public DocumentService(IDocumentRepository documentRepository, IUnitOfWork uow)
        {
            _documentRepository = documentRepository;
            _uow = uow;
        }
        
        public DocumentService(IDocumentRepository documentRepository, ICustomerRepository customerRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
            : this(documentRepository, uow)
        {
            this._customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region IDocumentService Members

        #region Add

        public GeneralResponse AddDocument(AddDocumentRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Document document = new Document();
                document.ID = Guid.NewGuid();
                document.CreateDate = PersianDateTime.Now;
                document.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                document.Customer = this._customerRepository.FindBy(request.CustomerID);
                document.DocumentName = request.DocumentName;
                document.Photo = request.Photo;
                document.Note = request.Note;
                document.RowVersion = 1;

                #region Rename The file

                // extract the extention
                var fileExtention = Path.GetExtension(request.Photo);
                // Get directory
                var directory = Path.GetDirectoryName(request.Photo);
                // create filename
                string fileName = directory + "/" + document.ID + fileExtention;
                // Rename file
                File.Move(request.Photo, fileName);

                #endregion
                
                document.ImageType = fileExtention;
                document.Photo = fileName;

                #region Validation

                if (document.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in document.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    File.Delete(fileName);

                    return response;
                }

                #endregion

                _documentRepository.Add(document);
                _uow.Commit();

                ////response.success = true;
                response.ID = document.ID;
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        #region One

        public GeneralResponse EditDocument(EditDocumentRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Document document = new Document();
            document = _documentRepository.FindBy(request.ID);

            if (document != null)
            {
                try
                {
                    document.ModifiedDate = PersianDateTime.Now;
                    document.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.DocumentName != null)
                        document.DocumentName = request.DocumentName;
                    if (request.ImageType != null)
                        document.ImageType = request.ImageType;
                    if (request.Photo != null)
                        document.Photo = request.Photo;
                    if (request.Note != null)
                        document.Note = request.Note;

                    if (document.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        document.RowVersion += 1;
                    }

                    if (document.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in document.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _documentRepository.Save(document);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {
                
                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }

        #endregion

        #endregion

        #region Delete

        public GeneralResponse DeleteDocument(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();


                try
                {
                    _documentRepository.RemoveById(request.ID);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }

            return response;
        }

        #endregion

        #region Get One

        public GetDocumentResponse GetDocument(GetRequest request)
        {
            GetDocumentResponse response = new GetDocumentResponse();

            try
            {
                Document document = new Document();
                DocumentView documentView = document.ConvertToDocumentView();

                document = _documentRepository.FindBy(request.ID);
                if (document != null)
                    documentView = document.ConvertToDocumentView();

                response.DocumentView = documentView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get Some

        //public GetDocumentsResponse GetDocuments()
        //{
        //    GetDocumentsResponse response = new GetDocumentsResponse();

        //    try
        //    {
        //        IEnumerable<DocumentView> documents = _documentRepository.FindAll()
        //            .ConvertToDocumentViews();

        //        response.DocumentViews = documents;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return response;
        //}

        public GetDocumentsResponse GetDocumentsBy(Guid customerID,IList<Sort> sort)
        {
            GetDocumentsResponse response = new GetDocumentsResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion = new Criterion("Customer.ID", customerID, CriteriaOperator.Equal);                
                
                query.Add(criterion);

                Response<Document> documents = _documentRepository.FindBy(query,-1,-1,sort);
                IEnumerable<DocumentView>  documentViews=documents.data.ConvertToDocumentViews();
                    
                // اصلاح آدرس عکسها و پاس دادن آدرس جدید
                foreach (var document in documentViews)
                {
                    document.Photo = document.Photo.Replace(@"\", "/").Substring(document.Photo.IndexOf("data")); ;
                }

                response.DocumentViews = documentViews;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #endregion

    }
}
