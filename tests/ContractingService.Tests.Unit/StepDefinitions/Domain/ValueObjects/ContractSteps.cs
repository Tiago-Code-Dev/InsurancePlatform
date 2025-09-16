using ContractingService.Domain.Entities;
using ContractingService.Domain.Enums;
using ContractingService.Domain.ValueObjects;
using ContractingService.Domain.Exceptions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace ContractingService.Tests.Unit.StepDefinitions.Domain.Entities
{
    [Binding]
    public class ContractSteps
    {
        private Contract? _contract;
        private Coverage? _coverage;
        private Insured? _insured;
        private Exception? _exception;

        [Given(@"I have a valid insured")]
        public void GivenIHaveAValidInsured()
        {
            _insured = new Insured(
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
            _contract = new Contract(insured);
        }

        [Given(@"I have a valid coverage")]
        public void GivenIHaveAValidCoverage()
        {
            _coverage = new Coverage("Life Coverage", CoverageType.Basic, new Money(100));
        }

        [Given(@"I activate the contract with the coverage")]
        public void GivenIActivateTheContractWithTheCoverage()
        {
            _contract.Should().NotBeNull();
            _coverage.Should().NotBeNull();

            _contract!.AddCoverage(_coverage!);
            _contract.Activate();
        }

        [Given(@"I have no insured")]
        public void GivenIHaveNoInsured()
        {
            _insured = null;
        }

        [When(@"I create the contract")]
        public void WhenICreateTheContract()
        {
            try
            {
                _contract = new Contract(_insured!);
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex; 
            }
        }

        [When(@"I add the coverage to the contract")]
        public void WhenIAddTheCoverageToTheContract()
        {
            _contract.Should().NotBeNull();
            _coverage.Should().NotBeNull();

            try
            {
                _contract!.AddCoverage(_coverage!);
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I try to activate the contract")]
        public void WhenITryToActivateTheContract()
        {
            _contract.Should().NotBeNull();

            try
            {
                _contract!.Activate();
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I try to add another coverage")]
        public void WhenITryToAddAnotherCoverage()
        {
            _contract.Should().NotBeNull();

            try
            {
                var anotherCoverage = new Coverage("Accident Coverage", CoverageType.Premium, new Money(200));
                _contract!.AddCoverage(anotherCoverage);
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I activate the contract with the coverage")]
        public void WhenIActivateTheContractWithTheCoverage()
        {
            _contract.Should().NotBeNull();
            _coverage.Should().NotBeNull();

            try
            {
                _contract!.AddCoverage(_coverage!);
                _contract.Activate();
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"the contract should be created successfully")]
        public void ThenTheContractShouldBeCreatedSuccessfully()
        {
            _contract.Should().NotBeNull();
            _contract!.Status.Should().Be(ContractStatus.Draft);
        }

        [Then(@"the coverage should be added successfully")]
        public void ThenTheCoverageShouldBeAddedSuccessfully()
        {
            _contract.Should().NotBeNull();
            _coverage.Should().NotBeNull();

            _contract!.Coverages.Should().ContainSingle(c => c.Name == _coverage!.Name);
        }

        [Then(@"a domain exception should be thrown with message ""(.*)""")]
        public void ThenADomainExceptionShouldBeThrownWithMessage(string expected)
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<DomainException>();
            _exception!.Message.Should().ContainEquivalentOf(expected);
        }

        [Then(@"a domain exception should be thrown containing message ""(.*)""")]
        public void ThenADomainExceptionShouldBeThrownContainingMessage(string expected)
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<DomainException>();
            _exception!.Message.Should().ContainEquivalentOf(expected);
        }

        [Then(@"the contract status should be ""(.*)""")]
        public void ThenTheContractStatusShouldBe(string expectedStatus)
        {
            _contract.Should().NotBeNull();
            _contract!.Status.ToString().Should().Be(expectedStatus);
        }

        [Then(@"the contract should have an activation date")]
        public void ThenTheContractShouldHaveAnActivationDate()
        {
            _contract.Should().NotBeNull();
            _contract!.ActivatedAt.Should().NotBeNull();
        }

        [When(@"I cancel the contract")]
        public void WhenICancelTheContract()
        {
            _contract.Should().NotBeNull();

            try
            {
                _contract!.Cancel();
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"the contract should be canceled")]
        public void ThenTheContractShouldBeCanceled()
        {
            _contract.Should().NotBeNull();
            _contract!.Status.ToString().Should().Be("Canceled");
        }

        [When(@"I terminate the contract")]
        public void WhenITerminateTheContract()
        {
            _contract.Should().NotBeNull();

            try
            {
                _contract!.Terminate();
            }
            catch (DomainException ex)
            {
                _exception = ex;
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"the contract should be terminated")]
        public void ThenTheContractShouldBeTerminated()
        {
            _contract.Should().NotBeNull();
            _contract!.Status.ToString().Should().Be("Terminated");
        }

        [Then(@"no exception should be thrown")]
        public void ThenNoExceptionShouldBeThrown()
        {
            _exception.Should().BeNull();
        }

        [Then(@"a domain exception should be thrown exactly with message ""(.*)""")]
        public void ThenADomainExceptionShouldBeThrownExactlyWithMessage(string expected)
        {
            _exception.Should().NotBeNull();
            _exception.Should().BeOfType<DomainException>();
            _exception!.Message.Should().Be(expected);
        }
    }
}
