using FluentAssertions;
using ProposalService.Domain.ValueObjects;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class MoneySteps
{
    private readonly ScenarioContext _context;

    public MoneySteps(ScenarioContext context)
    {
        _context = context;
    }

    private Money? _money;

    [Given(@"I have a money amount (.*)")]
    public void GivenIHaveAMoneyAmount(decimal amount)
    {
        _context["Amount"] = amount;
    }

    [When(@"I create the money")]
    public void WhenICreateTheMoney()
    {
        var amount = (decimal)_context["Amount"];

        try
        {
            _money = new Money(amount);
            _context["Money"] = _money;
        }
        catch (Exception ex)
        {
            _context["LastException"] = ex;
        }
    }

    [Then(@"the money should be created successfully")]
    public void ThenTheMoneyShouldBeCreatedSuccessfully()
    {
        var money = _context["Money"] as Money;
        money.Should().NotBeNull();
        money!.Amount.Should().Be((decimal)_context["Amount"]);
    }
}
