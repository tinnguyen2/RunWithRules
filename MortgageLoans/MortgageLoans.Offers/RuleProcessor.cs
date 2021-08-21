using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MortgageLoans.MortgageLoans.Offers
{
    //  Current Design support 1 Rule -> 1 Change
    //  To support 1 rule -> multiple change, the fields "ProductPropertyAffected, ProductPropertyChangedAmount"
    //      will need to be convert into a list of Tuple<string, string>
    //  To support multiple rule -> 1 change we can 
    //      creatively using RuleApplyOrder and undefined Product Property Affected,
    //      and add another layer of "OfferRule" processing

    public class RuleProcessor:IRuleProcessor
    {
        private ILogger<RuleProcessor> mLogger;
        public RuleProcessor(ILogger<RuleProcessor> logger)
        {
            mLogger = logger;
        }
        public Offer RunRules(Applicant applicant, Product product)
        {
             var offer = new Offer()
            {
                ApplicantId = applicant.ApplicantId,
                ProductId = product.ProductId,
                OfferDetails = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            };
            foreach(var detail in product.ProductDetails)
            {
                offer.OfferDetails[detail.ProductDetailPropertyName] = detail.ProductDetailProperValue;
            }
            foreach(var rule in product.ProductRules.OrderBy(p => p.RuleApplyOrder))
            {
                if(rule.RuleTextMatch(applicant.TextValueProperties) || rule.RuleNumericMatch(applicant.NumericValueProperties))
                {
                    if (!offer.OfferDetails.ContainsKey(rule.ProductPropertyAffected))
                    {
                        //throw new Exception("Bad Rule Definition: No Property Name " + rule.ProductPropertyAffected + " in Product " + product.ProductName);
                        mLogger.LogWarning("This ProductPropertyAffected: [" + rule.ProductPropertyAffected + "] is not defined in product detail. You are better know what you are doing");

                       
                        offer.OfferDetails[rule.ProductPropertyAffected] = rule.ProductPropertyChangeAmount;
                    }
                    mLogger.LogDebug("Rule Matched: " + rule.RuleName.ToString());
                    mLogger.LogDebug("Adjusting Offer property: " + rule.ProductPropertyAffected + " by the amount of " + rule.ProductPropertyChangeAmount);
                    
                    offer.OfferDetails[rule.ProductPropertyAffected] += rule.ProductPropertyChangeAmount;
                    
                }
            }
            if (offer.OfferDetails["InterestRate"] > 1000)
                offer.Disqualified = true;
            return offer;
        }
    }
}
