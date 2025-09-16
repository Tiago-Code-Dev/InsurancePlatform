using FluentAssertions;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.ValueObjects;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class ContractSteps
{
    private readonly ScenarioContext _context;

    public ContractSteps(ScenarioContext context)
    {
        _context = context;
    }

    private Contract? _contract;

    [Given(@"I have a valid contract type")]
    public void GivenIHaveAValidContractType()
    {
        _context["ContractType"] = ContractType.Standard;
    }

    [Given(@"I have a premium amount (.*)")]
    public void GivenIHaveAPremiumAmount(decimal amount)
    {
        _context["Premium"] = amount;
    }

    [Given(@"I have a start date today")]
    public void GivenIHaveAStartDateToday()
    {
        _context["StartDate"] = DateTime.UtcNow.Date;
    }

    [Given(@"I have an end date one year from today")]
    public void GivenIHaveAnEndDateOneYearFromToday()
    {
        _context["EndDate"] = DateTime.UtcNow.Date.AddYears(1);
    }

    [Given(@"I have an end date yesterday")]
    public void GivenIHaveAnEndDateYesterday()
    {
        _context["EndDate"] = DateTime.UtcNow.Date.AddDays(-1);
    }

    [When(@"I create the contract")]
    public void WhenICreateTheContract()
    {
        var type = (ContractType)_context["ContractType"];
        var amount = (decimal)_context["Premium"];
        var startDate = (DateTime)_context["StartDate"];
        var endDate = (DateTime)_context["EndDate"];

        try
        {
            _contract = new Contract(type, new Money(amount), startDate, endDate);
            _context["Contract"] = _contract;
        }
        catch (Exception ex)
        {
            _context["LastException"] = ex;
        }
    }

    [Then(@"the contract should be created successfully")]
    public void ThenTheContractShouldBeCreatedSuccessfully()
    {
        var contract = _context["Contract"] as Contract;
        contract.Should().NotBeNull();

        contract!.Premium.Amount.Should().BeGreaterThanOrEqualTo(0);

        contract.EndDate.Should().BeOnOrAfter(contract.StartDate);
    }
}
