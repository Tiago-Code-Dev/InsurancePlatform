using ContractingService.Domain.Entities;
using ContractingService.Domain.Enums;
using ContractingService.Domain.Exceptions;
using ContractingService.Domain.ValueObjects;
using ContractingService.Tests.Unit.Shared.Assertions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace ContractingService.Tests.Unit.StepDefinitions.Domain.Entities
{
    [Binding]
    public class ContractSteps
    {
        private Contract? _currentContract;
        private Coverage? _currentCoverage;
        private Insured? _currentInsured;
        private Exception? _caughtException;

        private Contract RequireContract()
            => _currentContract ?? throw new InvalidOperationException("Contract not initialized in this scenario.");
        private Coverage RequireCoverage()
            => _currentCoverage ?? throw new InvalidOperationException("Coverage not initialized in this scenario.");
        private Insured RequireInsured()
            => _currentInsured ?? throw new InvalidOperationException("Insured not initialized in this scenario.");

        [Given(@"I have a valid insured")]
        public void GivenIHaveAValidInsured()
        {
            _currentInsured = new Insured(
                "John Doe",
                new Document("12345678901"),
                new Email("john@doe.com"),
                Guid.NewGuid() 
            );
        }

        [Given(@"I have a valid contract")]
        public void GivenIHaveAValidContract()
        {
            var insured = new Insured("John Doe", new Document("12345678901"), new Email("john@doe.com"), Guid.NewGuid());
            _currentContract = new Contract(insured);
        }

        [Given(@"I have a valid coverage")]
        public void GivenIHaveAValidCoverage()
        {
            _currentCoverage = new Coverage("Life Coverage", CoverageType.Basic, new Money(100));
        }

        [Given(@"I have no insured")]
        public void GivenIHaveNoInsured()
        {
            _currentInsured = null;
        }

        [When(@"I create the contract")]
        public void WhenICreateTheContract()
        {
            try
            {
                var insured = RequireInsured();
                _currentContract = new Contract(insured);
            }
            catch (Exception ex) { _caughtException = ex; }
        }

        [When(@"I add the coverage to the contract")]
        public void WhenIAddTheCoverageToTheContract()
        {
            try
            {
                var contract = RequireContract();
                var coverage = RequireCoverage();
                contract.AddCoverage(coverage);
            }
            catch (Exception ex) { _caughtException = ex; }
        }

        [When(@"I try to activate the contract")]
        public void WhenITryToActivateTheContract()
        {
            try { RequireContract().Activate(); }
            catch (Exception ex) { _caughtException = ex; }
        }

        [When(@"I try to add another coverage")]
        public void WhenITryToAddAnotherCoverage()
        {
            try
            {
                var another = new Coverage("Accident Coverage", CoverageType.Premium, new Money(200));
                RequireContract().AddCoverage(another);
            }
            catch (Exception ex) { _caughtException = ex; }
        }

        [Given(@"I activate the contract with the coverage")]
        [When(@"I activate the contract with the coverage")]
        public void ActivateTheContractWithTheCoverage()
        {
            var contract = RequireContract();
            var coverage = RequireCoverage();

            if (!contract.Coverages.Any())
                contract.AddCoverage(coverage);

            contract.Activate();
        }

        [When(@"I cancel the contract")]
        public void WhenICancelTheContract()
        {
            try { RequireContract().Cancel(); }
            catch (Exception ex) { _caughtException = ex; }
        }

        [When(@"I terminate the contract")]
        public void WhenITerminateTheContract()
        {
            try { RequireContract().Terminate(); }
            catch (Exception ex) { _caughtException = ex; }
        }

        [Then(@"the contract should be created successfully")]
        public void ThenTheContractShouldBeCreatedSuccessfully()
        {
            RequireContract().Status.Should().Be(ContractStatus.Draft);
        }

        [Then(@"the coverage should be added successfully")]
        public void ThenTheCoverageShouldBeAddedSuccessfully()
        {
            var contract = RequireContract();
            var coverage = RequireCoverage();
            contract.Coverages.Should().ContainSingle(c => c.Name == coverage.Name);
        }

        [Then(@"a domain exception should be thrown with message ""(.*)""")]
        public void ThenADomainExceptionShouldBeThrownWithMessage(string expected)
        {
            _caughtException.Should().NotBeNull();
            _caughtException.Should().BeOfType<DomainException>();
            _caughtException!.Message.Should().ContainEquivalentOf(expected);
        }

        [Then(@"a domain exception should be thrown containing message ""(.*)""")]
        public void ThenADomainExceptionShouldBeThrownContainingMessage(string expected)
        {
            _caughtException.Should().NotBeNull();
            _caughtException.Should().BeOfType<DomainException>();
            _caughtException!.Message.Should().ContainEquivalentOf(expected);
        }

        [Then(@"the contract status should be ""(.*)""")]
        public void ThenTheContractStatusShouldBe(string expectedStatus)
        {
            RequireContract().Status.ToString().Should().Be(expectedStatus);
        }

        [Then(@"the contract should have an activation date")]
        public void ThenTheContractShouldHaveAnActivationDate()
        {
            RequireContract().ActivatedAt.Should().NotBeNull();
        }

        [Then(@"the contract should be canceled")]
        public void ThenTheContractShouldBeCanceled()
        {
            RequireContract().Status.ToString().Should().Be("Canceled");
        }

        [Then(@"the contract should be terminated")]
        public void ThenTheContractShouldBeTerminated()
        {
            RequireContract().Status.ToString().Should().Be("Terminated");
        }

        [Then(@"no exception should be thrown")]
        public void ThenNoExceptionShouldBeThrown()
        {
            _caughtException.Should().BeNull();
        }

        [Then(@"a domain exception should be thrown exactly with message ""(.*)""")]
        public void ThenADomainExceptionShouldBeThrownExactlyWithMessage(string expected)
        {
            _caughtException.Should().NotBeNull();
            _caughtException.Should().BeOfType<DomainException>();
            _caughtException!.Message.Should().Be(expected);
        }

        [Then(@"an operation exception should be thrown with message ""(.*)""")]
        public void ThenAnOperationExceptionShouldBeThrownWithMessage(string aliasKey)
        {
            _caughtException.Should().NotBeNull();
            var ex = _caughtException!;
            ex.Should().BeOfType<InvalidOperationException>();
            MessageAlias.AssertMatches(ex.Message, aliasKey, AliasGroup.Insured);
        }
    }
}
