# RunWithRules
A demo for a basic rule engine for mortgage loans.
This is an ASP.Core Web API project. You may need to run dotnet restore or manually restore some nuget package to run.
When you run it, you can use Swagger UI page to execute various methods.
The main method is "GetOffer" which is expected a Applicant Id and a Product Id and will provide a json response with Interest Rate and other features of a loan.
If the interest rate is higher than "1000" the "Disqualified" flag will be set to true
This project provided 4 sample applicants (Id 1 to 4) and 3 sample products (Id 1 to 3) with various rules stored in Sample data under CSV format
