using System;
using System.Collections.Generic;

namespace MortgageLoans.MortgageLoans.Offers
{
    public class ProductRule
    {
        public int ProductRuleId { get; set; }
        public int ProductId { get; set; }
        public string RuleName { get; set; }
        public int RuleApplyOrder { get; set; }
        public string ApplicantPropertyName { get; set; }
        public decimal ApplicantPropertyNumericValue { get; set; }
        public string ApplicantPropertyStringValue { get; set; }
        public string CompareFunction { get; set; }
        public string ProductPropertyAffected { get; set; }
        public decimal ProductPropertyChangeAmount { get; set; }
        

        public bool RuleTextMatch(Dictionary<string, string> inputs)
        {
            foreach (var input in inputs)
            {
                if (input.Key.ToUpper() == ApplicantPropertyName.ToUpper())
                {
                    var matched = input.Value.ToUpper() == ApplicantPropertyStringValue.ToUpper();
                    switch (CompareFunction.ToUpper())
                    {
                        case "SAME":
                            return matched;
                        case "DIFF":
                            return !matched;
                    };
                }

            }
            return false;
        }
        public bool RuleNumericMatch(Dictionary<string, decimal> inputs)
        {
            foreach (var input in inputs)
            {
                if (input.Key.ToUpper() == ApplicantPropertyName.ToUpper())
                {
                    switch (CompareFunction.ToUpper())
                    {
                        case "EQUAL":
                            return input.Value == ApplicantPropertyNumericValue;
                        case "LESS":
                            return input.Value < ApplicantPropertyNumericValue;
                        case "MORE":
                            return input.Value > ApplicantPropertyNumericValue;
                        case "LESSOREQUAL":
                            return input.Value <= ApplicantPropertyNumericValue;
                        case "MOREOREQUAL":
                            return input.Value >= ApplicantPropertyNumericValue;
                    }
                }

            }
            return false;
        }
        public ProductRule()
        {
        }
    }
}
