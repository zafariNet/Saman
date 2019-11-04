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
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class DocumentStatusService : IDocumentStatusService
    {
        #region Declares

        private readonly IDocumentStatusRepository _documentStatusRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region ctor

        public DocumentStatusService(IDocumentStatusRepository documentStatusRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _documentStatusRepository = documentStatusRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region IDocumentStatusService Members

        #region Add

        #region One

        public GeneralResponse AddDocumentStatus(AddDocumentStatusRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                DocumentStatus documentStatus = new DocumentStatus();
                documentStatus.ID = Guid.NewGuid();
                documentStatus.CreateDate = PersianDateTime.Now;
                documentStatus.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                documentStatus.DefaultStatus = request.DefaultStatus;
                documentStatus.CompleteStatus = request.CompleteStatus;
                documentStatus.DocumentStatusName = request.DocumentStatusName;
                documentStatus.SortOrder = GetSortOrder();
                documentStatus.RowVersion = 1;

                #region Validation

                if (documentStatus.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in documentStatus.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _documentStatusRepository.Add(documentStatus);
                _uow.Commit();

                ////response.success = true;
                response.ID = documentStatus.ID;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Some

        public GeneralResponse AddDocumentStatuss(IEnumerable<AddDocumentStatusRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            bool defaultFlag = false;
            bool completeFlag = false;
            Guid defaultDocumentStatusID = Guid.Empty;
            Guid completeDocumentStatusID = Guid.Empty;

            try
            {
                foreach (AddDocumentStatusRequest request in requests)
                {
                    DocumentStatus documentStatus = new DocumentStatus();
                    documentStatus.ID = Guid.NewGuid();
                    documentStatus.CreateDate = PersianDateTime.Now;
                    documentStatus.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);

                    #region If Default or Complete status changed, change all entity to remain only one of this fields true

                    documentStatus.DefaultStatus = request.DefaultStatus;
                    if (request.DefaultStatus)
                    {
                        defaultFlag = true;
                        defaultDocumentStatusID = documentStatus.ID;
                    }
                    
                    documentStatus.CompleteStatus = request.CompleteStatus;
                    if (request.CompleteStatus)
                    {
                        completeFlag = true;
                        completeDocumentStatusID = documentStatus.ID;
                    }

                    #endregion

                    documentStatus.DocumentStatusName = request.DocumentStatusName;
                    documentStatus.SortOrder = GetSortOrder();
                    documentStatus.RowVersion = 1;

                    #region Validation

                    if (documentStatus.GetBrokenRules().Count() > 0)
                    {
                        

                        foreach (BusinessRule businessRule in documentStatus.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _documentStatusRepository.Add(documentStatus);
                }

                #region if default or complete flag is true change all entity to remain one of them true

                if (defaultFlag)
                {
                    foreach (var document_Status in _documentStatusRepository.FindAll())
                    {
                        if (document_Status.ID == defaultDocumentStatusID)
                            document_Status.DefaultStatus = true;
                        else
                            document_Status.DefaultStatus = false;

                        _documentStatusRepository.Save(document_Status);
                    }
                }

                if (completeFlag)
                {
                    foreach (var document_Status in _documentStatusRepository.FindAll())
                    {
                        if (document_Status.ID == completeDocumentStatusID)
                            document_Status.CompleteStatus = true;
                        else
                            document_Status.CompleteStatus = false;

                        _documentStatusRepository.Save(document_Status);
                    }
                }

                #endregion

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

        #endregion

        #region Edit

        #region One

        public GeneralResponse EditDocumentStatus(EditDocumentStatusRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            DocumentStatus documentStatus = new DocumentStatus();
            documentStatus = _documentStatusRepository.FindBy(request.ID);

            if (documentStatus != null)
            {
                try
                {
                    documentStatus.ModifiedDate = PersianDateTime.Now;
                    documentStatus.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    documentStatus.DefaultStatus = request.DefaultStatus;
                    documentStatus.CompleteStatus = request.CompleteStatus;
                    if (request.DocumentStatusName != null)
                        documentStatus.DocumentStatusName = request.DocumentStatusName;

                    if (documentStatus.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        documentStatus.RowVersion += 1;
                    }

                    if (documentStatus.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in documentStatus.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _documentStatusRepository.Save(documentStatus);
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

        #region Some

        public GeneralResponse EditDocumentStatuss(IEnumerable<EditDocumentStatusRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();
            DocumentStatus documentStatus = new DocumentStatus();

            bool defaultFlag = false;
            bool completeFlag = false;
            Guid defaultDocumentStatusID = Guid.Empty;
            Guid completeDocumentStatusID = Guid.Empty;

            try
            {
                foreach (EditDocumentStatusRequest request in requests)
                {
                    documentStatus = _documentStatusRepository.FindBy(request.ID);

                    documentStatus.ModifiedDate = PersianDateTime.Now;
                    documentStatus.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);

                    #region If Default or Complete status changed, change all entity to remain only one of this fields true

                    if (documentStatus.DefaultStatus != request.DefaultStatus)
                    {
                        documentStatus.DefaultStatus = request.DefaultStatus;
                        defaultFlag = true;
                        if (documentStatus.DefaultStatus) defaultDocumentStatusID = documentStatus.ID;
                    }
                    if (documentStatus.CompleteStatus != request.CompleteStatus)
                    {
                        documentStatus.CompleteStatus = request.CompleteStatus;
                        completeFlag = true;
                        if (documentStatus.CompleteStatus) completeDocumentStatusID = documentStatus.ID;
                    }

                    #endregion

                    documentStatus.DocumentStatusName = request.DocumentStatusName;

                    #region RowVersion

                    if (documentStatus.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        documentStatus.RowVersion += 1;
                    }

                    #endregion

                    #region Validation

                    if (documentStatus.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in documentStatus.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _documentStatusRepository.Save(documentStatus);
                }

                #region if default or complete flag is true change all entity to remain one of them true

                if (defaultFlag)
                {
                    foreach (var document_Status in _documentStatusRepository.FindAll())
                    {
                        if (document_Status.ID == defaultDocumentStatusID)
                            document_Status.DefaultStatus = true;
                        else
                            document_Status.DefaultStatus = false;

                        _documentStatusRepository.Save(document_Status);
                    }
                }

                if (completeFlag)
                {
                    foreach (var document_Status in _documentStatusRepository.FindAll())
                    {
                        if (document_Status.ID == completeDocumentStatusID)
                            document_Status.CompleteStatus = true;
                        else
                            document_Status.CompleteStatus = false;

                        _documentStatusRepository.Save(document_Status);
                    }
                }

                #endregion

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

        #endregion

        #region Delete

        public GeneralResponse DeleteDocumentStatus(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();
                try
                {
                    _documentStatusRepository.RemoveById(request.ID);
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

        public GetDocumentStatusResponse GetDocumentStatus(GetRequest request)
        {
            GetDocumentStatusResponse response = new GetDocumentStatusResponse();

            try
            {
                DocumentStatus documentStatus = new DocumentStatus();
                DocumentStatusView documentStatusView = documentStatus.ConvertToDocumentStatusView();

                documentStatus = _documentStatusRepository.FindBy(request.ID);
                if (documentStatus != null)
                    documentStatusView = documentStatus.ConvertToDocumentStatusView();

                response.DocumentStatusView = documentStatusView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get All

        public GetDocumentStatussResponse GetDocumentStatuss()
        {
            GetDocumentStatussResponse response = new GetDocumentStatussResponse();

            try
            {
                IEnumerable<DocumentStatusView> documentStatuss = _documentStatusRepository.FindAll()
                    .OrderBy(o => o.SortOrder)
                    .ConvertToDocumentStatusViews();

                response.DocumentStatusViews = documentStatuss;
            }
            catch (Exception ex)
            {
                throw;
            }


            return response;
        }

        public GetGeneralResponse<IEnumerable<DocumentStatusView>> GetDocumentStatuss(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<DocumentStatusView>> response = new GetGeneralResponse<IEnumerable<DocumentStatusView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                
                Response<DocumentStatus> documentStatuss = _documentStatusRepository.FindAll(index, count);

                response.data = documentStatuss.data.OrderBy(o => o.SortOrder).ConvertToDocumentStatusViews();
                response.totalCount = documentStatuss.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }


            return response;
        }

        #endregion

        #region Move

        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            DocumentStatus currentDocumentStatus = new DocumentStatus();
            currentDocumentStatus = _documentStatusRepository.FindBy(request.ID);

            // Find the Previews Agency
            DocumentStatus previewsDocumentStatus = new DocumentStatus();
            try
            {
                previewsDocumentStatus = _documentStatusRepository.FindAll()
                                .Where(s => s.SortOrder < currentDocumentStatus.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentDocumentStatus != null && previewsDocumentStatus != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentDocumentStatus.SortOrder;
                    int previews = (int)previewsDocumentStatus.SortOrder;

                    currentDocumentStatus.SortOrder = previews;
                    previewsDocumentStatus.SortOrder = current;

                    _documentStatusRepository.Save(currentDocumentStatus);
                    _documentStatusRepository.Save(previewsDocumentStatus);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            DocumentStatus currentDocumentStatus = new DocumentStatus();
            currentDocumentStatus = _documentStatusRepository.FindBy(request.ID);

            // Find the Previews Agency
            DocumentStatus nextDocumentStatus = new DocumentStatus();
            try
            {
                nextDocumentStatus = _documentStatusRepository.FindAll()
                                .Where(s => s.SortOrder > currentDocumentStatus.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            if (currentDocumentStatus != null && nextDocumentStatus != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentDocumentStatus.SortOrder;
                    int previews = (int)nextDocumentStatus.SortOrder;

                    currentDocumentStatus.SortOrder = previews;
                    nextDocumentStatus.SortOrder = current;

                    _documentStatusRepository.Save(currentDocumentStatus);
                    _documentStatusRepository.Save(nextDocumentStatus);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }
        #endregion

        #endregion

        #region Private Members

        private int GetSortOrder()
        {
            try
            {
                IEnumerable<DocumentStatus> documentStatuss = _documentStatusRepository.FindAll();
                return documentStatuss.Max(s => (int)s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        #endregion

    }
}
