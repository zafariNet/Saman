using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Store
{
    public class ProductView : BaseView
    {
        public Guid ProductCategoryID { get; set; }

        [Display(Name = "نام دسته")]
        public string ProductCategoryName { get; set; }

        [Display(Name = "نام کالا")]
        public string ProductName { get; set; }

        [Display(Name = "کد")]
        public int ProductCode { get; set; }

        [Display(Name = "تعداد")]
        public int UnitsInStock { get; set; }

        [Display(Name = "غیر فعال")]
        public bool Discontinued { get; set; }

        [Display(Name = "توضیحات")]
        public string Note { get; set; }
        
        

        public int SortOrder { get; set; }
        //IEnumerables
        
        //[Display(Name = "فروش کالا")]
        //public IEnumerable<Sales.ProductSaleDetailView> ProductSaleDetails { get; protected set; }
        
        //[Display(Name = "تحویل کالاها")]
        //public IEnumerable<ProductDeliveryView> ProductDeliverys { get; protected set; }
        
        //[Display(Name = "لاگ کالاها")]
        //public IEnumerable<ProductLogView> ProductLogs { get; protected set; }
        
        //[Display(Name = "قیمتها")]
        //public IEnumerable<ProductPriceView> ProductPrices { get; protected set; }
        
        [Display(Name = "انبار کالاها")]
        public IEnumerable<StoreProductView> StoreProducts { get; protected set; }

        public int TodalInStores { get; set; }

        public int Total { get {
            return UnitsInStock + TodalInStores;
        } }
        //{ get {
        //    //return ProductLogs.Sum(x => x.UnitsIO);
        //    return StoreProducts.Sum(x => x.UnitsInStock);
        //} }
    }
}
