using System;
namespace MortgageLoans.MortgageLoans.Offers
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductDetailPropertyName { get; set; }
        public decimal ProductDetailProperValue { get; set; }
        public ProductDetail()
        {
        }
    }
}
