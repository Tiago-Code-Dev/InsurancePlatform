using FluentAssertions;
using ProposalService.Domain.Exceptions;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class CommonSteps
{
    private readonly ScenarioContext _context;

    public CommonSteps(ScenarioContext context)
    {
        _context = context;
    }

    [Then(@"a domain exception should be thrown")]
    public void ThenADomainExceptionShouldBeThrown()
    {
        var exception = _context.ContainsKey("LastException")
            ? _context["LastException"] as Exception
            : null;

        exception.Should().NotBeNull();
        exception.Should().BeOfType<DomainException>();
    }
}
