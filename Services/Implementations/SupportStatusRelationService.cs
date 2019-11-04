using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Support;
using Model.Support.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportStatusRelationService : ISupportStatusRelationService
    {
        #region Declare

        private readonly ICustomerRepository _customerRepository;
        private readonly ISupportStatusRepository _supportStatusRepository;
        private readonly ISupportStatusRelationRepository _supportStatusRelationRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public SupportStatusRelationService(ICustomerRepository customerRepository,
            ISupportStatusRepository supportStatusRepository,
            ISupportStatusRelationRepository supportStatusRelationRepository, IUnitOfWork uow)
        {
            this._customerRepository = customerRepository;
            this._supportStatusRepository = supportStatusRepository;
            this._supportStatusRelationRepository = supportStatusRelationRepository;
            this._uow = uow;
        }

        #endregion

        #region Read

        public GetGeneralResponse<IEnumerable<SupportStatusRelationView>> GetSupportStatuseRelations()
        {
            GetGeneralResponse<IEnumerable<SupportStatusRelationView>> response =
    new GetGeneralResponse<IEnumerable<SupportStatusRelationView>>();

            try
            {

                IEnumerable<SupportStatusRelation> supportStatusRelations = _supportStatusRelationRepository.FindAll();

                response.data = supportStatusRelations.ConvertToSupportStatusRelationViews();
                response.totalCount = supportStatusRelations.Count();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException.Message != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<SupportStatusRelationView>> GetSupportStatuseRelations(
            Guid SupportStatusID)
        {
            GetGeneralResponse<IEnumerable<SupportStatusRelationView>> response =
                new GetGeneralResponse<IEnumerable<SupportStatusRelationView>>();

            try
            {


                IEnumerable<SupportStatusRelation> supportStatusRelations = _supportStatusRelationRepository.FindAll().Where(x=>x.SupportStatus.ID==SupportStatusID);

                response.data = supportStatusRelations.ConvertToSupportStatusRelationViews();
                response.totalCount = supportStatusRelations.Count();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException.Message!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Add

        public GeneralResponse AddSupportStatusRelation(IEnumerable<SupportStatusRelationView> requests, Guid SupportStatusID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (SupportStatusRelationView request in requests)
                {
                    SupportStatusRelation supportStatusRelation = new SupportStatusRelation();

                    supportStatusRelation.ID = Guid.NewGuid();
                    supportStatusRelation.RelatedSupportStatus =
                        _supportStatusRepository.FindBy(request.ID);
                    supportStatusRelation.SupportStatus = _supportStatusRepository.FindBy(SupportStatusID);

                    _supportStatusRelationRepository.Add(supportStatusRelation);  
                }

                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!= null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditSupportStatusRelation(IEnumerable<EditSupportStatusRelationRequest> requests,Guid SupportStatusID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                foreach (EditSupportStatusRelationRequest request in requests)
                {
                    SupportStatusRelation supportStatusRelation = new SupportStatusRelation();
                    
                    supportStatusRelation = _supportStatusRelationRepository.FindBy(request.ID);
                    SupportStatus supportStatus = _supportStatusRepository.FindBy(SupportStatusID);
                    SupportStatus NextSupportStatus = _supportStatusRepository.FindBy(request.SupportStatusRelatedID);

                    supportStatusRelation.SupportStatus = supportStatus;
                    supportStatusRelation.RelatedSupportStatus = NextSupportStatus;

                    _supportStatusRelationRepository.Add(supportStatusRelation);
 
                }

                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }
        #endregion

        #region Delete

        public GeneralResponse DeleteSupportStatusRelations(IEnumerable<SupportStatusRelationView> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (SupportStatusRelationView request in requests)
                {
                    SupportStatusRelation supportStatusRelation=new SupportStatusRelation();



                    supportStatusRelation = _supportStatusRelationRepository.FindBy(request.ID);
                    _supportStatusRelationRepository.Remove(supportStatusRelation);
                }

                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion
    }
}
