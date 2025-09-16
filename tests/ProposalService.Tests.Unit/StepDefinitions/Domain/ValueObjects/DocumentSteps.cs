using FluentAssertions;
using ProposalService.Domain.ValueObjects;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions;

[Binding]
public class DocumentSteps
{
    private readonly ScenarioContext _context;

    public DocumentSteps(ScenarioContext context)
    {
        _context = context;
    }

    private Document? _document;

    [Given(@"I have a document number ""(.*)""")]
    public void GivenIHaveADocumentNumber(string number)
    {
        _context["Number"] = number;
    }

    [When(@"I create the document")]
    public void WhenICreateTheDocument()
    {
        var number = (string)_context["Number"];

        try
        {
            _document = new Document(number);
            _context["Document"] = _document;
        }
        catch (Exception ex)
        {
            _context["LastException"] = ex; 
        }
    }

    [Then(@"the document should be created successfully")]
    public void ThenTheDocumentShouldBeCreatedSuccessfully()
    {
        var doc = _context["Document"] as Document;
        doc.Should().NotBeNull();
        doc!.Number.Should().Be((string)_context["Number"]);
    }
}
