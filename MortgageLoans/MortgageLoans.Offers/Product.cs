using System;
using System.Collections.Generic;

namespace MortgageLoans.MortgageLoans.Offers
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        
        public List<ProductDetail> ProductDetails { get; set; }
        public List<ProductRule> ProductRules { get; set; }
        public Product()
        {
            ProductDetails = new List<ProductDetail>();
            ProductRules = new List<ProductRule>();
            
        }
        public void AddDetail(ProductDetail productDetail)
        {
            //TODO: Check for Unique Field
            ProductDetails.Add(productDetail);
        }
        public void AddRule(ProductRule productRule)
        {
            //TODO: Check for Unique Rule
            ProductRules.Add(productRule);
        }
    }
}
