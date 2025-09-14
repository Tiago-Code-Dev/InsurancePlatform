using FluentAssertions;
using ProposalService.Domain.Entities;
using ProposalService.Domain.ValueObjects;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class CustomerSteps
{
    private readonly ScenarioContext _context;

    public CustomerSteps(ScenarioContext context)
    {
        _context = context;
    }

    private Customer? _customer;

    [Given(@"I have a customer name ""(.*)""")]
    public void GivenIHaveACustomerName(string name)
    {
        _context["CustomerName"] = name;
    }

    [Given(@"I have a valid document number ""(.*)""")]
    public void GivenIHaveAValidDocumentNumber(string number)
    {
        _context["CustomerDocument"] = number;
    }

    [Given(@"I have a valid email ""(.*)""")]
    public void GivenIHaveAValidEmail(string email)
    {
        _context["CustomerEmail"] = email;
    }

    [When(@"I create the customer")]
    public void WhenICreateTheCustomer()
    {
        var name = (string)_context["CustomerName"];
        var document = (string)_context["CustomerDocument"];
        var email = (string)_context["CustomerEmail"];

        try
        {
            _customer = new Customer(name, new Document(document), new Email(email));
            _context["Customer"] = _customer;
        }
        catch (Exception ex)
        {
            _context["LastException"] = ex;
        }
    }

    [Then(@"the customer should be created successfully")]
    public void ThenTheCustomerShouldBeCreatedSuccessfully()
    {
        var customer = _context["Customer"] as Customer;
        customer.Should().NotBeNull();
        customer!.Name.Should().NotBeNullOrWhiteSpace();
        customer.Document.Number.Length.Should().BeInRange(11, 14);
        customer.Email.Address.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}

