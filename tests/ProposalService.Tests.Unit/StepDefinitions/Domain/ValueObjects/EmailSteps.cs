using FluentAssertions;
using ProposalService.Domain.ValueObjects;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class EmailSteps
{
    private readonly ScenarioContext _context;

    public EmailSteps(ScenarioContext context)
    {
        _context = context;
    }

    private Email? _email;

    [Given(@"I have an email address ""(.*)""")]
    public void GivenIHaveAnEmailAddress(string address)
    {
        _context["Address"] = address;
    }

    [When(@"I create the email")]
    public void WhenICreateTheEmail()
    {
        var address = (string)_context["Address"];

        try
        {
            _email = new Email(address);
            _context["Email"] = _email;
        }
        catch (Exception ex)
        {
            _context["LastException"] = ex; 
        }
    }

    [Then(@"the email should be created successfully")]
    public void ThenTheEmailShouldBeCreatedSuccessfully()
    {
        var email = _context["Email"] as Email;
        email.Should().NotBeNull();
        email!.Address.Should().Be((string)_context["Address"]);
    }
}
