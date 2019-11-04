using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Employees;
using Model.Employees.Interfaces;
using NHibernate.Dialect;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Employees;
using Services.Mapping;
namespace Services.Implementations
{
    public class MessageTemplateService : IMessageTemplateService
    {
        #region Declare

        public readonly IEmployeeRepository _employeeRepository;
        public readonly IMessageTemplateRepository _messageTemplateRepository;
        private readonly IUnitOfWork _uow;

        #endregion
        
        #region Ctor

        public MessageTemplateService(IEmployeeRepository employeeRepository, IMessageTemplateRepository messageTemplateRepository, IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _messageTemplateRepository = messageTemplateRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        public GetGeneralResponse<IEnumerable<MessageTemplateView>> GetMessageTemplates(int? MessageType, int pageSize,
            int pageNumber)
        {
            GetGeneralResponse<IEnumerable<MessageTemplateView>> response=new GetGeneralResponse<IEnumerable<MessageTemplateView>>();

            try
            {
                int index = (pageNumber - 1)*pageSize;
            int count = pageSize;

            Response<MessageTemplate> messageTemplates=new Response<MessageTemplate>();
            if (MessageType != null)
            {
                Query query=new Query();
                Criterion messageTypeCriteria = new Criterion("MessageType", MessageType, CriteriaOperator.Equal);
                query.Add(messageTypeCriteria);
                 messageTemplates = _messageTemplateRepository.FindBy(query, index, count);
                
            }
            else
            {
                messageTemplates = _messageTemplateRepository.FindAll(index, count);
            }

            response.data = messageTemplates.data.ConvertToMessageTemplateViews();
            response.totalCount = messageTemplates.totalCount;

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

        #region Add

        public GeneralResponse AddMessageTemplates(AddMessageTemplateRequest request,Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                if (request != null)
                {


                    MessageTemplate messageTemplate = new MessageTemplate();

                    messageTemplate.ID = Guid.NewGuid();
                    messageTemplate.MessageTemplateName = request.MessageTemplateName;
                    messageTemplate.CreateDate = PersianDateTime.Now;
                    messageTemplate.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    messageTemplate.RowVersion = 1;
                    messageTemplate.MessageSmsTemplateText = request.MessageSmsTemplateText;
                    messageTemplate.MessageEmailTemplateText = request.MessageEmailTemplateText;
                    

                    _messageTemplateRepository.Add(messageTemplate);
                    _uow.Commit();
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GeneralResponse AddEmailToMessageTemplate(Guid MessageTemplateID, string Message,Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                MessageTemplate messageTemplate=new MessageTemplate();

                messageTemplate = _messageTemplateRepository.FindBy(MessageTemplateID);
                messageTemplate.ModifiedDate = PersianDateTime.Now;
                messageTemplate.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                messageTemplate.MessageEmailTemplateText = Message;
                _messageTemplateRepository.Save(messageTemplate);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        public GeneralResponse AddSmsToMessageTemplate(Guid MessageTemplateID, string Message, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                MessageTemplate messageTemplate = new MessageTemplate();

                messageTemplate = _messageTemplateRepository.FindBy(MessageTemplateID);
                messageTemplate.ModifiedDate = PersianDateTime.Now;
                messageTemplate.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                messageTemplate.MessageSmsTemplateText = Message;
                
                _messageTemplateRepository.Save(messageTemplate);
                _uow.Commit();
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

        public GeneralResponse EditMessageTemplates(IEnumerable<EditeMessageTemplateRequest> requests,Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                if (requests != null)
                {
                    foreach (EditeMessageTemplateRequest request in requests)
                    {


                        MessageTemplate messageTemplate = _messageTemplateRepository.FindBy(request.ID);

                        messageTemplate.MessageTemplateName = request.MessageTemplateName;
                        messageTemplate.ModifiedDate = PersianDateTime.Now;
                        messageTemplate.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);

                        _messageTemplateRepository.Save(messageTemplate);
                    }
                    _uow.Commit();
                }

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

        public GeneralResponse DeleteMessageTemplate(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var item in requests)
                {

                    _messageTemplateRepository.RemoveById(item.ID);
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
