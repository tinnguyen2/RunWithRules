using Microsoft.Extensions.Logging;
using MortgageLoans.MortgageLoans.Offers;
using NUnit.Framework;
using Serilog;

namespace MortgageLoanTester
{
    public class Tests
    {
        RuleProcessor mRuleProcessor { get; set; }
        
        [SetUp]
        public void Setup()
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<RuleProcessor>();

            mRuleProcessor = new RuleProcessor(logger);
            
        }

        [Test]
        public void DisqualifiedWhenInterestRateIs100000()
        {
            var app1 = new Applicant();
            app1.TextValueProperties["State"] = "FL";
            var product1 = new Product();
            product1.AddDetail(new ProductDetail{
                ProductDetailPropertyName = "InterestRate",
                ProductDetailProperValue = 5
            });
            product1.AddRule(new ProductRule
            {
                RuleName = "RuleName1",
                ApplicantPropertyName = "state",
                ApplicantPropertyStringValue = "fl",
                CompareFunction = "SAME",
                ProductPropertyAffected = "interestrate",
                ProductPropertyChangeAmount = 1000000
            });
            var offer = mRuleProcessor.RunRules(app1, product1);
            Assert.IsTrue( offer.Disqualified);
        }
        [Test]
        public void ReduceInterestRateForGoodCreditInFlorida()
        {
            var app1 = new Applicant();
            app1.TextValueProperties["State"] = "FL";
            app1.NumericValueProperties["creditscore"] = 725;
            var product1 = new Product();
            product1.AddDetail(new ProductDetail
            {
                ProductDetailPropertyName = "InterestRate",
                ProductDetailProperValue = 5
            });
            product1.AddRule(new ProductRule
            {
                RuleName = "RuleNameA",
                ApplicantPropertyName = "state",
                ApplicantPropertyStringValue = "fl",
                ProductPropertyAffected = "interestrate",
                CompareFunction = "SAME",
                ProductPropertyChangeAmount = (decimal)-0.5,
                RuleApplyOrder = 1

            });

            product1.AddRule(new ProductRule
            {
                RuleName = "RuleNameB",
                ApplicantPropertyName = "creditSCORE",
                ApplicantPropertyNumericValue = 760,
                ProductPropertyAffected = "interestrate",
                ProductPropertyChangeAmount = (decimal)-2,
                RuleApplyOrder = 2,
                CompareFunction = "moreORequal"

            });

            product1.AddRule(new ProductRule
            {
                RuleName = "RuleNameC",
                ApplicantPropertyName = "creditSCORE",
                ApplicantPropertyNumericValue = 720,
                ProductPropertyAffected = "interestrate",
                ProductPropertyChangeAmount = (decimal)-0.3,
                RuleApplyOrder = 3,
                CompareFunction = "more"
            });
            var offer = mRuleProcessor.RunRules(app1, product1);
            Assert.IsFalse (offer.Disqualified);
            Assert.AreEqual(4.2, offer.OfferDetails["InterestRate"]);
        }
    }
}
