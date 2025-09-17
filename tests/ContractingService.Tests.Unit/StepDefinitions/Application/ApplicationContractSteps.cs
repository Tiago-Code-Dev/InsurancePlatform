using ContractingService.Application.DTOs;
using ContractingService.Application.Interfaces;
using ContractingService.Application.Services;
using ContractingService.Domain.Enums;
using ContractingService.Tests.Unit.Support.Fakes;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.CrossCutting.Response;
using ContractingService.Tests.Unit.Support;
using System.Linq;
using TechTalk.SpecFlow;

namespace ContractingService.Tests.Unit.StepDefinitions.Application.Contracts
{
    [Binding, Scope(Tag = "app-contract")]
    internal class AppContractSteps
    {
        private Guid _currentContractId;
        private FakeContractRepository _repo = default!;
        private IContractAppService _appService = default!;
        private readonly Mock<IExternalApiService> _externalMock = new();
        private readonly Mock<ILogger<ContractAppService>> _loggerMock = new();
        private CustomResponse<ContractDto>? _lastContractResponse; 
        private CustomResponse<Result>? _lastResultResponse;

        public AppContractSteps()
        {
            _repo = new FakeContractRepository();
            _appService = new ContractAppService(
                _repo,
                _externalMock.Object,
                _loggerMock.Object); 
        }

        [Given(@"an existing draft contract with one coverage")]
        public async Task GivenDraftWithCoverage()
        {
            var insured = new InsuredDto("John Doe", "52998224725", "john@doe.com", Guid.NewGuid()); // CPF válido
            var cov = new CoverageDto("Base", CoverageType.Basic.ToString(), 100.00m); // tipo 100% compatível + 2 casas
            var resp = await _appService.CreateAsync(insured, new List<CoverageDto> { cov });

            resp.Success.Should().BeTrue(resp.Dump());   

            _lastContractResponse = resp;
            _lastResultResponse = null;
            _currentContractId = resp.Data!.Id;
        }

        [Given(@"an existing draft contract without coverage")]
        public async Task GivenDraftWithoutCoverage()
        {
            var insured = new InsuredDto("John Doe", "52998224725", "john@doe.com", Guid.NewGuid());
            var resp = await _appService.CreateAsync(insured, new List<CoverageDto>());

            resp.Success.Should().BeTrue(resp.Dump());   // idem

            _lastContractResponse = resp;
            _lastResultResponse = null;
            _currentContractId = resp.Data!.Id;
        }


        [Given(@"there is no current contract in repository")]
        public void GivenNoCurrentContract()
        {
            _currentContractId = Guid.NewGuid();     
            _lastContractResponse = null;
            _lastResultResponse = null;
        }

        [Given(@"an active contract \(created via application\)")]
        public async Task GivenActiveContract()
        {
            await GivenDraftWithCoverage();
            _lastResultResponse = await _appService.ActivateAsync(_currentContractId);
            _lastResultResponse.Success.Should().BeTrue();
            _repo.ClearCounters();
        }

        [Given(@"an already terminated contract")]
        public async Task GivenTerminatedContract()
        {
            await GivenDraftWithoutCoverage();
            _lastResultResponse = await _appService.TerminateAsync(_currentContractId);
            _lastResultResponse.Success.Should().BeTrue();
            _repo.ClearCounters();
        }

        [When(@"the application activates the current contract")]
        public async Task WhenAppActivates()
        {
            _lastResultResponse = await _appService.ActivateAsync(_currentContractId); 
            _lastContractResponse = null;
        }

        [When(@"the application cancels the current contract")]
        public async Task WhenAppCancels()
        {
            _lastResultResponse = await _appService.CancelAsync(_currentContractId);   
            _lastContractResponse = null;
        }

        [When(@"the application terminates the current contract")]
        public async Task WhenAppTerminates()
        {
            _lastResultResponse = await _appService.TerminateAsync(_currentContractId);
            _lastContractResponse = null;
        }

        [Then(@"the app response should be successful")]
        public void ThenAppResponseShouldBeSuccessful()
        {
            if (_lastResultResponse is not null)
                _lastResultResponse.Success.Should().BeTrue();
            else
                _lastContractResponse!.Success.Should().BeTrue();
        }

        [Then(@"the app response should be an internal server error")]
        public void ThenAppResponseShouldBeInternalServerError()
        {
            if (_lastResultResponse is not null)
                _lastResultResponse.Success.Should().BeFalse();
            else
                _lastContractResponse!.Success.Should().BeFalse();
        }

        [Then(@"the repository should have persisted the contract with status ""(.*)""")]
        public async Task ThenRepoPersistedWithStatus(string status)
        {
            _repo.UpdateCount.Should().BeGreaterThan(0);
            var contract = await _repo.GetByIdAsync(_currentContractId);
            contract.Should().NotBeNull();
            contract!.Status.ToString().Should().Be(status);
        }

        [Then(@"the repository should not have persisted any change")]
        public void ThenRepoNoPersist()
        {
            _repo.UpdateCount.Should().Be(0);
        }

        [When(@"the application tries to create a contract with name ""(.*)"" doc ""(.*)"" email ""(.*)"" and (.*) coverages")]
        public async Task WhenAppTriesCreateWithInvalidInsured(string name, string doc, string email, int coverages)
        {
            var insured = new InsuredDto(name, doc, email, Guid.NewGuid());
            var list = new List<CoverageDto>();
            for (int i = 0; i < coverages; i++)
                list.Add(new CoverageDto($"C{i}", "Basic", 100m));

            _lastContractResponse = await _appService.CreateAsync(insured, list); 
            _lastResultResponse = null;
        }

        [When(@"the application tries to create a contract with (.*) coverage named ""(.*)"" type ""(.*)"" premium (.*)")]
        public async Task WhenAppTriesCreateWithInvalidCoverage(int count, string name, string type, decimal premium)
        {
            var insured = new InsuredDto("John Doe", "12345678901", "john@doe.com", Guid.NewGuid());
            var list = new List<CoverageDto>();
            for (int i = 0; i < count; i++)
                list.Add(new CoverageDto(name, type, premium));

            _lastContractResponse = await _appService.CreateAsync(insured, list); 
            _lastResultResponse = null;
        }


        [Then(@"the repository should have persisted a contract with (.*) coverage")]
        public async Task ThenRepoPersistedCoverageCount(int expected)
        {
            _repo.UpdateCount.Should().BeGreaterThan(0);
            var contract = await _repo.GetByIdAsync(_currentContractId);
            contract.Should().NotBeNull();
            contract!.Coverages.Should().HaveCount(expected);
        }
    }
}
