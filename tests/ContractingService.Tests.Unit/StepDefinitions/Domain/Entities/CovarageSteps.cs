using ContractingService.Domain.Entities;
using ContractingService.Domain.Enums;
using ContractingService.Domain.Exceptions;
using ContractingService.Domain.ValueObjects;
using ContractingService.Tests.Unit.Shared.Assertions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace ContractingService.Tests.Unit.StepDefinitions.Domain.Entities
{
    [Binding, Scope(Tag = "coverage")]
    public class CoverageSteps
    {
        private Coverage? _coverage;
        private Contract? _contract;
        private Exception? _ex;

        [Given(@"a coverage draft named ""(.*)"" priced as (.*)")]
        public void GivenCoverageDraft(string name, decimal premium)
        {
            try
            {
                _coverage = new Coverage(name, CoverageType.Basic, new Money(premium));
            }
            catch (Exception e) { _ex = e; }
        }

        [When(@"the coverage draft is finalized for validation")]
        public void WhenCoverageDraftIsFinalized()
        {
            // A validação ocorre no construtor; este step mantém o fluxo BDD.
        }

        [Given(@"a fresh draft contract exists")]
        public void GivenFreshDraftContract()
        {
            var insured = new Insured("John Doe", new Document("12345678901"), new Email("john@doe.com"),Guid.NewGuid());
            _contract = new Contract(insured);
        }

        [When(@"the draft coverage is attached to the contract")]
        public void WhenCoverageAttachedToContract()
        {
            _contract.Should().NotBeNull();
            _coverage.Should().NotBeNull();

            try { _contract!.AddCoverage(_coverage!); }
            catch (Exception e) { _ex = e; }
        }

        [Then(@"the contract should include exactly one coverage")]
        public void ThenContractHasExactlyOneCoverage()
        {
            _contract.Should().NotBeNull();
            _contract!.Coverages.Should().HaveCount(1);
        }

        [Given(@"the contract is activated with the current coverage")]
        public void GivenContractActivatedWithCurrentCoverage()
        {
            _contract.Should().NotBeNull();
            _coverage.Should().NotBeNull();
            _contract!.AddCoverage(_coverage!);
            _contract.Activate();
        }

        [When(@"a second coverage attempt is performed")]
        public void WhenSecondCoverageAttempt()
        {
            _contract.Should().NotBeNull();
            try
            {
                var second = new Coverage("Extra", CoverageType.Premium, new Money(200));
                _contract!.AddCoverage(second);
            }
            catch (Exception e) { _ex = e; }
        }

        [Then(@"a domain error should mention ""(.*)""")]
        public void ThenDomainErrorMentions(string key)
        {
            _ex.Should().NotBeNull();
            MessageAlias.AssertMatches(_ex!.Message, key, AliasGroup.Coverage);
        }

        [Then(@"no domain error should occur")]
        public void ThenNoDomainErrorShouldOccur()
        {
            _ex.Should().BeNull();
        }
    }
}
