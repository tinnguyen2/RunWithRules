using System;
using System.Collections.Generic;

namespace MortgageLoans.MortgageLoans.Offers
{
    public class Applicant
    {
        public int ApplicantId { get; set; }
        public Dictionary<string, string> TextValueProperties { get; set; }
        public Dictionary<string, decimal> NumericValueProperties { get; set; }
        public Applicant()
        {
            TextValueProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            NumericValueProperties = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
