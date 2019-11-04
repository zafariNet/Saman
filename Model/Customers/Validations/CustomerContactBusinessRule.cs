using Model.Base;

namespace Model.Customers.Validations
{
    public class CustomerContactBusinessRule
    {
        public static readonly BusinessRule CustomerContactDesciptionRequired=new BusinessRule("Description","توضیحات را وارد کنید");
        public static readonly BusinessRule CustomerRequired=new BusinessRule("Customer","انتخاب مشتری الزامیست.");
        public static readonly BusinessRule CustomerContactTemplateRequired = new BusinessRule("Customer", "انتخاب نتایجه تماس الزامیست.");
    }
}
