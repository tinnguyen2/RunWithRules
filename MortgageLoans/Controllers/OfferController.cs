using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MortgageLoans.MortgageLoans.Offers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MortgageLoans.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfferController : Controller
    {
        // GET: /<controller>/
        private IConfiguration mConfig { get; set; }
        private Dictionary<int, Applicant> mApplicants { get; set; }
        private Dictionary<int, Product> mProducts { get; set; }
        private IRuleProcessor mRuleProcessor {get;set;}
        public OfferController(IConfiguration config, IRuleProcessor ruleProcessor, ILogger<OfferController> logger)
        {
            mConfig = config;
            GetApplicants();
            GetProducts();
            mRuleProcessor = ruleProcessor;
        }

        

        [HttpGet]
        [Route("GetOffer")]
        public Offer GetOffer(int applicantId, int productId)
        {
            
            var offer = mRuleProcessor.RunRules(mApplicants[applicantId], mProducts[productId]);
            return offer;
        }

        [HttpGet]
        [Route("GetApplicants")]

        public List<Applicant> GetApplicants()
        {
            var customers = new Dictionary<int, Applicant>();
            var numericLines = System.IO.File.ReadAllLines(mConfig["ApplicantNumericValuePropertiesFile"]);
            foreach(var l in numericLines)
            {
                if (l.Contains("ApplicantId"))
                    continue;
                var flds = l.Split(',');
                var a = new Applicant();
                a.ApplicantId = Int32.Parse(flds[0]);
                if (!customers.ContainsKey(a.ApplicantId))
                {
                    customers[a.ApplicantId] = a;
                    a.NumericValueProperties = new Dictionary<string, decimal>();
                    a.TextValueProperties = new Dictionary<string, string>();
                }
                customers[a.ApplicantId].NumericValueProperties[flds[1]] =  decimal.Parse(flds[2]);
            }
            var lines = System.IO.File.ReadAllLines(mConfig["ApplicantTextValuePropertiesFile"]);
            foreach (var l in lines)
            {
                if (l.Contains("ApplicantId"))
                    continue;
                var flds = l.Split(',');
                var a = new Applicant();
                a.ApplicantId = Int32.Parse(flds[0]);
                if (!customers.ContainsKey(a.ApplicantId))
                {
                    customers[a.ApplicantId] = a;
                    a.NumericValueProperties = new Dictionary<string, decimal>();
                    a.TextValueProperties = new Dictionary<string, string>();
                }
                customers[a.ApplicantId].TextValueProperties[flds[1]] = flds[2];
            }
            mApplicants = customers;
            return customers.Values.ToList();
        }
        [HttpGet]
        [Route("GetProducts")]
        public List<Product> GetProducts()
        {
            var products = new Dictionary<int, Product>();
            var lines = System.IO.File.ReadAllLines(mConfig["ProductFile"]);
            foreach (var l in lines)
            {
                if (l.Contains("Id"))
                    continue;
                var flds = l.Split(',');
                var p = new Product();
                p.ProductId = Int32.Parse(flds[0]);
                p.ProductName = flds[1];
                if (!products.ContainsKey(p.ProductId))
                {
                    products[p.ProductId] = p;
                    p.ProductDetails = new List<ProductDetail>();
                    p.ProductRules = new List<ProductRule>();
                
                }
            }
            var detailLines = System.IO.File.ReadAllLines(mConfig["ProductDetailFile"]);
            foreach (var l in detailLines)
            {
                if (l.Contains("Id"))
                    continue;
                var flds = l.Split(',');
                
                var productId = Int32.Parse(flds[1]);
                
                if (products.ContainsKey(productId))
                {
                    products[productId].ProductDetails.Add(new ProductDetail {
                        ProductDetailId =Int32.Parse(flds[0]),
                        ProductId = Int32.Parse(flds[1]),
                        ProductDetailPropertyName = flds[2],
                        ProductDetailProperValue = decimal.Parse(flds[3])
                    });
                   
                }
            }
            var ruleLines = System.IO.File.ReadAllLines(mConfig["ProductRuleFile"]);
            foreach (var l in ruleLines)
            {
                if (l.Contains("Id"))
                    continue;
                var flds = l.Split(',');

                var productId = Int32.Parse(flds[1]);

                if (products.ContainsKey(productId))
                {
                    products[productId].ProductRules.Add(new ProductRule
                    {
                        ProductRuleId = Int32.Parse(flds[0]),
                        ProductId = Int32.Parse(flds[1]),
                        RuleName = flds[2],
                        RuleApplyOrder = Int32.Parse(flds[3]),
                        ApplicantPropertyName = flds[4],
                        ApplicantPropertyNumericValue = decimal.Parse(flds[5]),
                        ApplicantPropertyStringValue = flds[6],
                        CompareFunction = flds[7],
                        ProductPropertyAffected = flds[8],
                        ProductPropertyChangeAmount =  decimal.Parse(flds[9])
                    });

                }
            }
            mProducts = products;
            return products.Values.ToList();
        }
    }
}
