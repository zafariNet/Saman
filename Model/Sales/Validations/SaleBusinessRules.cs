using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Sales.Validations
{
    public class SaleBusinessRules
    {
        public static readonly BusinessRule CustomerRequired = new BusinessRule("Customer", "مشتری باید وارد شود");
        public static readonly BusinessRule ThisIsRollback = new BusinessRule("", "این فاکتور برگشت از فروش است و برای آن نمی توان فاکتور برگشت از فروش صادر کرد.");
        public static readonly BusinessRule ThisIsRollbackAndCantDelete = new BusinessRule("", "این فاکتور برگشت از فروش است و برای آن نمی توان اقلام آن را حذف کرد.");
        public static readonly BusinessRule SomeProductsNotExists = new BusinessRule("", "برخی از کالاهای موجود در فاکتور برگشتی در فاکتور اصلی وجود ندارد");
        public static readonly BusinessRule SomeCreditsNotExists = new BusinessRule("", "برخی از خدمات اعتباری موجود در فاکتور برگشتی در فاکتور اصلی وجود ندارد");
        public static readonly BusinessRule SomeUncreditsNotExists = new BusinessRule("", "برخی از خدمات غیر اعتباری موجود در فاکتور برگشتی در فاکتور اصلی وجود ندارد.");
        public static readonly BusinessRule SomeProductsBeforeRollbacked = new BusinessRule("", "برخی از کالاهای موجود در فاکتور برگشتی قبلاً برگشت شده اند.");
        public static readonly BusinessRule SomeCreditsBeforeRollbacked = new BusinessRule("", "برخی از خدمات اعتباری موجود در فاکتور برگشتی قبلاً برگشت شده اند.");
        public static readonly BusinessRule SomeUncreditsBeforeRollbacked = new BusinessRule("", "برخی از خدمات غیر اعتباری موجود در فاکتور برگشتی قبلاً برگشت شده اند.");
        public static readonly BusinessRule RollbackCreditGreaterThanMain = new BusinessRule("", "مبلغ برگشتی نمی تواند بیش از مبلغ فاکتور باشد");
        public static readonly BusinessRule RollbackCreditkGreaterThanMain = new BusinessRule("", "مبلغ برگشتی به شبکه نمی تواند بیش از قیمت خرید آیتم مورد نظر باشد باشد");
        public static readonly BusinessRule RollbackUncreditGreaterThanMain = new BusinessRule("", "مبلغ برگشتی نمی تواند بیش از مبلغ فاکتور باشد");
        public static readonly BusinessRule RollbackProductGreaterThanMain = new BusinessRule("", "مبلغ برگشتی نمی تواند بیش از مبلغ فاکتور باشد");
        public static readonly BusinessRule ThisIsRollbackCannotDeliver = new BusinessRule("", "این فاکتور مربوط به برگشت از فروش بوده و آیتم های آن قابل تحویل نیست.");
        public static readonly BusinessRule AlreadyDeliverd = new BusinessRule("", "این آیتم قبلا تحویل شده است.");
        public static readonly BusinessRule SaleIsNotClosedCantDeliver = new BusinessRule("", "فاکتور جاری تأیید نشده است و قابل تحویل نیست.");
        public static readonly BusinessRule NetworkBalanceNotEnough = new BusinessRule("", "موجودی شبکه کافی نیست و اجازه تحویل هنگام کافی نبودن موجودی صادر نشده است.");
        public static readonly BusinessRule StoresNotContainThisProduct = new BusinessRule("", "تحویل انجام نشد. هیچ کدام از انبارهای متعلق به شما، شامل این کالا نیست.");
        public static readonly BusinessRule SaleClosed = new BusinessRule("", "فاکتور جاری تأیید و بسته شده است و امکان انجام این عملیات وجود ندارد.");
        public static readonly BusinessRule UnitsInStockNotEnough = new BusinessRule("", "تحویل انجام نشد. موجودی انبارهای متعلق به شما کافی نمی باشد و کالا قابل تحویل نیست.");
        public static readonly BusinessRule RollbackedAndCantDeliver = new BusinessRule("", "این آیتم قبلا برگشت خورده و قابل تحویل نیست.");

        public static readonly BusinessRule LineTotalwithoutDscountAndImpositionIsLess = new BusinessRule("", "خطا در مبلغ خالص - مبلغ خالص کمتر از تعداد در قیمت واحد میباشد. با برنامه نویس تماس بگیرید.");
        public static readonly BusinessRule LineTotalIsGrater = new BusinessRule("", "خطا در مبلغ برگشتی - مبلغ برگشتی بیش از مبلغ فروش است . لطفا با برنامه نویس تماس بگیرید.");
        public static readonly BusinessRule LineImpositionError=new BusinessRule("LineImposition","جمع نهایی مالیات صحیح نیست - لطفا با برنامه نویس تماس بگیرید.");
        public static readonly BusinessRule LineDiscountnError = new BusinessRule("LineDiscount", "جمع نهایی تخفیف صحیح نیست - لطفا با برنامه نویس تماس بگیرید.");
        public static readonly BusinessRule LineDiscountnGrater = new BusinessRule("LineDiscount", "تخفیف فاکتور بیش از تخفیف مجاز است - با برنامه نویس تماس بگیرید.");
        public static readonly BusinessRule LineimpositionGrater = new BusinessRule("LineImposition", "مالیات فاکتور بیش از تخفیف مجاز است - با برنامه نویس تماس بگیرید.");
        
    }
}
