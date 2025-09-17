using FluentAssertions;
using Moq;
using ProposalService.Application.DTOs;
using ProposalService.Application.Integration;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Interfaces;
using ProposalService.Domain.ValueObjects;
using Shared.CrossCutting.Messaging.Events;
using Shared.CrossCutting.Response;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;

namespace ProposalService.Tests.Unit.StepDefinitions.Application.Services;

[Binding]
public class ProposalAppServiceSteps
{
    private readonly ScenarioContext _context;
    private readonly Mock<IProposalRepository> _repositoryMock;
    private readonly Mock<IProposalIntegrationEventPublisher> _publisherMock;
    private readonly ProposalAppService _service;
    private readonly Mock<ILogger<ProposalAppService>> _loggerMock;
    private CustomResponse<ProposalDto>? _proposalResponse;
    private CustomResponse<Result>? _resultResponse;

    public ProposalAppServiceSteps(ScenarioContext context)
    {
        _context = context;
        _repositoryMock = new Mock<IProposalRepository>();
        _publisherMock = new Mock<IProposalIntegrationEventPublisher>();
        _loggerMock = new Mock<ILogger<ProposalAppService>>();

        _service = new ProposalAppService(
        _repositoryMock.Object,
        _publisherMock.Object,
        _loggerMock.Object);
    }

    [Given(@"I have a valid customer DTO")]
    public void GivenIHaveAValidCustomerDto()
    {
        _context["CustomerDto"] = new CustomerDto
        {
            Name = "John Doe",
            Document = "12345678901",
            Email = "john.doe@email.com"
        };
    }

    [Given(@"I have a customer DTO with document ""(.*)""")]
    public void GivenIHaveACustomerDtoWithDocument(string doc)
    {
        _context["CustomerDto"] = new CustomerDto
        {
            Name = "Jane Doe",
            Document = doc,
            Email = "jane.doe@email.com"
        };
    }

    [Given(@"I have a customer DTO with email ""(.*)""")]
    public void GivenIHaveACustomerDtoWithEmail(string email)
    {
        _context["CustomerDto"] = new CustomerDto
        {
            Name = "Jane Doe",
            Document = "12345678901",
            Email = email
        };
    }

    [Given(@"I have a valid contract DTO")]
    public void GivenIHaveAValidContractDto()
    {
        _context["ContractDto"] = new ContractDto
        {
            Type = "Standard",
            Premium = 100,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };
    }

    [Given(@"I have a contract DTO with premium (.*)")]
    public void GivenIHaveAContractDtoWithPremium(decimal premium)
    {
        _context["ContractDto"] = new ContractDto
        {
            Type = "Standard",
            Premium = premium,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1)
        };
    }

    [Given(@"a proposal exists in the repository")]
    public void GivenAProposalExistsInTheRepository()
    {
        var proposal = new Proposal(
            new Customer("Alice", new Document("12345678901"), new Email("alice@email.com")),
            new Contract(ContractType.Standard, new Money(100), DateTime.UtcNow, DateTime.UtcNow.AddYears(1))
        );

        _repositoryMock.Setup(r => r.GetByIdAsync(proposal.Id)).ReturnsAsync(proposal);
        _context["ProposalId"] = proposal.Id;
    }

    [Given(@"no proposal exists with the requested ID")]
    public void GivenNoProposalExistsWithTheRequestedId()
    {
        var fakeId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(fakeId)).ReturnsAsync((Proposal?)null);
        _context["ProposalId"] = fakeId;
    }

    [When(@"I call the service to create the proposal")]
    public async Task WhenICallTheServiceToCreateTheProposal()
    {
        var customerDto = (CustomerDto)_context["CustomerDto"];
        var contractDto = (ContractDto)_context["ContractDto"];

        _proposalResponse = await _service.CreateAsync(customerDto, contractDto);
    }

    [When(@"I call the service to get the proposal by ID")]
    public async Task WhenICallTheServiceToGetTheProposalById()
    {
        var id = (Guid)_context["ProposalId"];
        _proposalResponse = await _service.GetByIdAsync(id);
    }

    [When(@"I call the service to approve the proposal")]
    public async Task WhenICallTheServiceToApproveTheProposal()
    {
        var id = (Guid)_context["ProposalId"];
        _resultResponse = await _service.ApproveAsync(id);
    }

    [When(@"I call the service to reject the proposal")]
    public async Task WhenICallTheServiceToRejectTheProposal()
    {
        var id = (Guid)_context["ProposalId"];
        _resultResponse = await _service.RejectAsync(id);
    }

    [Then(@"the response should be created successfully")]
    public void ThenTheResponseShouldBeCreatedSuccessfully()
    {
        _proposalResponse.Should().NotBeNull();
        _proposalResponse!.Success.Should().BeTrue();
        _proposalResponse.Data.Should().NotBeNull();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Proposal>()), Times.Once);
        _publisherMock.Verify(p => p.PublishAsync(It.IsAny<ProposalCreatedEvent>()), Times.Once);
    }

    [Then(@"the response should fail with message ""(.*)""")]
    public void ThenTheResponseShouldFailWithMessage(string expectedMessage)
    {
        if (_proposalResponse != null)
        {
            _proposalResponse.Success.Should().BeFalse();
            _proposalResponse.Messages.Should().NotBeEmpty();
            _proposalResponse.Messages.First().Message.Should().Contain(expectedMessage);
        }
        else if (_resultResponse != null)
        {
            _resultResponse.Success.Should().BeFalse();
            _resultResponse.Messages.Should().NotBeEmpty();
            _resultResponse.Messages.First().Message.Should().Contain(expectedMessage);
        }
    }

    [Then(@"the response should return the proposal successfully")]
    public void ThenTheResponseShouldReturnTheProposalSuccessfully()
    {
        _proposalResponse.Should().NotBeNull();
        _proposalResponse!.Success.Should().BeTrue();
        _proposalResponse.Data.Should().NotBeNull();
    }

    [Then(@"the proposal should be approved")]
    public void ThenTheProposalShouldBeApproved()
    {
        _resultResponse.Should().NotBeNull();
        _resultResponse!.Success.Should().BeTrue();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Once);
    }

    [Then(@"the proposal should be rejected")]
    public void ThenTheProposalShouldBeRejected()
    {
        _resultResponse.Should().NotBeNull();
        _resultResponse!.Success.Should().BeTrue();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Once);
    }

    [Then(@"the repository AddAsync should be called")]
    public void ThenTheRepositoryAddAsyncShouldBeCalled()
    {
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Proposal>()), Times.Once);
    }

    [Then(@"the event publisher PublishAsync should be called")]
    public void ThenTheEventPublisherPublishAsyncShouldBeCalled()
    {
        _publisherMock.Verify(p => p.PublishAsync(It.IsAny<ProposalCreatedEvent>()), Times.Once);
    }

    [Then(@"the repository UpdateAsync should be called")]
    public void ThenTheRepositoryUpdateAsyncShouldBeCalled()
    {
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Proposal>()), Times.Once);
    }
}
