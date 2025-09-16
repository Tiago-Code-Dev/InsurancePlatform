using FluentAssertions;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.ValueObjects;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class ProposalSteps
{
    private readonly ScenarioContext _context;

    public ProposalSteps(ScenarioContext context)
    {
        _context = context;
    }

    private Proposal? _proposal;

    [Given(@"I have a valid customer")]
    public void GivenIHaveAValidCustomer()
    {
        var customer = new Customer("John Doe", new Document("12345678901"), new Email("john.doe@email.com"));
        _context["Customer"] = customer;
    }

    [Given(@"I have no customer")]
    public void GivenIHaveNoCustomer()
    {
        _context["Customer"] = null!;
    }

    [Given(@"I have a valid contract")]
    public void GivenIHaveAValidContract()
    {
        var contract = new Contract(
            ContractType.Standard,
            new Money(100),
            DateTime.UtcNow,
            DateTime.UtcNow.AddYears(1)
        );
        _context["Contract"] = contract;
    }

    [Given(@"I have no contract")]
    public void GivenIHaveNoContract()
    {
        _context["Contract"] = null!;
    }

    [When(@"I create the proposal")]
    public void WhenICreateTheProposal()
    {
        var customer = _context.ContainsKey("Customer") ? _context["Customer"] as Customer : null;
        var contract = _context.ContainsKey("Contract") ? _context["Contract"] as Contract : null;

        try
        {
            _proposal = new Proposal(customer!, contract!);
            _context["Proposal"] = _proposal;
        }
        catch (Exception ex)
        {
            _context["LastException"] = ex;
        }
    }

    [Then(@"the proposal should be created successfully")]
    public void ThenTheProposalShouldBeCreatedSuccessfully()
    {
        var proposal = _context["Proposal"] as Proposal;
        proposal.Should().NotBeNull();

        proposal!.Customer.Should().NotBeNull();
        proposal.Customer.Name.Should().NotBeNullOrWhiteSpace();
        proposal.Customer.Document.Number.Length.Should().BeInRange(11, 14);
        proposal.Customer.Email.Address.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        proposal.Contract.Should().NotBeNull();
        proposal.Contract.Premium.Amount.Should().BeGreaterThanOrEqualTo(0);
    }

    [Then(@"the proposal should have initial status ""(.*)""")]
    public void ThenTheProposalShouldHaveInitialStatus(string expectedStatus)
    {
        var proposal = _context["Proposal"] as Proposal;
        proposal!.Status.ToString().Should().Be(expectedStatus);
    }
}
