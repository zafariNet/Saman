using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sales;
using Services.ViewModels.Sales;
using AutoMapper;

namespace Services.Mapping
{
    public static class SaleMapper
    {
        public static IEnumerable<SaleView> ConvertToSaleViews(
            this IEnumerable<Sale> sales)
        {
            List<SaleView> returnSaleViews = new List<SaleView>();

            foreach (Sale sale in sales)
            {
                returnSaleViews.Add(sale.ConvertToSaleView());
            }

            return returnSaleViews;
        }

        public static SaleView ConvertToSaleView(this Sale sale)
        {
            return Mapper.Map<Sale, SaleView>(sale);
        }

        public static IEnumerable<SimpleSaleView> ConvertToSimpleSaleViews(
         this IEnumerable<Sale> sales)
        {
            List<SimpleSaleView> returnSaleViews = new List<SimpleSaleView>();

            foreach (Sale sale in sales)
            {
                returnSaleViews.Add(sale.ConvertToSimpleSaleView());
            }

            return returnSaleViews;
        }

        public static SimpleSaleView ConvertToSimpleSaleView(this Sale sale)
        {
            return Mapper.Map<Sale, SimpleSaleView>(sale);
        }
    }
}