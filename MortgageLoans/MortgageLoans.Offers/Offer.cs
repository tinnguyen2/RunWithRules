using System;
using System.Collections.Generic;

namespace MortgageLoans.MortgageLoans.Offers
{
    //Base From Product with Changed Properties according to the rule
    public class Offer
    {
        public int ApplicantId { get; set; }
        public int ProductId { get; set; }
        public Dictionary<string, decimal> OfferDetails { get; set; }
        public bool Disqualified { get; set; }
        public Offer()
        {
            OfferDetails = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
