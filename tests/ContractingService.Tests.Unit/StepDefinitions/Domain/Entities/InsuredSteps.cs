using ContractingService.Domain.Entities;
using ContractingService.Domain.ValueObjects;
using ContractingService.Tests.Unit.Shared.Assertions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace ContractingService.Tests.Unit.StepDefinitions.Domain.Entities
{
    [Binding, Scope(Tag = "insured")]
    public class InsuredSteps
    {
        private Insured? _insured;
        private Exception? _ex;

        [Given(@"an insured blueprint with name ""(.*)"" document ""(.*)"" email ""(.*)""")]
        public void GivenInsuredBlueprint(string name, string doc, string email)
        {
            try
            {
                _insured = new Insured(name, new Document(doc), new Email(email),Guid.NewGuid());
            }
            catch (Exception e) { _ex = e; }
        }

        [When(@"the insured blueprint is materialized")]
        public void WhenInsuredBlueprintIsMaterialized()
        {
            // Construção já ocorreu no Given; step sem ação intencional.
        }

        [Then(@"a domain error should mention ""(.*)""")]
        public void ThenDomainErrorMentions(string key)
        {
            _ex.Should().NotBeNull();
            MessageAlias.AssertMatches(_ex!.Message, key, AliasGroup.Insured);
        }
    }
}
