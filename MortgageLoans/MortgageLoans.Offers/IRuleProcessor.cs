namespace MortgageLoans.MortgageLoans.Offers
{
    public interface IRuleProcessor
    {
        Offer RunRules(Applicant applicant, Product product);
    }
}