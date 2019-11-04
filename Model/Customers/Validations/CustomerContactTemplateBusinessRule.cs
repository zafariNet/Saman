using Model.Base;

namespace Model.Customers.Validations
{
    public class CustomerContactTemplateBusinessRule
    {
        public static readonly BusinessRule CustomerContactTemplateTitleRequired=new BusinessRule("Title","وارد کردن عنوان تماس الزامیست");
    }
}
