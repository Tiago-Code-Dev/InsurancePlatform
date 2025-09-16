using ContractingService.Domain.Exceptions;
using ContractingService.Domain.ValueObjects;
using ContractingService.Tests.Unit.Shared.Assertions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TechTalk.SpecFlow;

namespace ContractingService.Tests.Unit.StepDefinitions.Domain.ValueObjects
{
    [Binding, Scope(Tag = "valueobjects")]
    internal class ValueObjectsSteps
    {
        private Document? _document;
        private Email? _email;
        private Money? _money;
        private Exception? _ex;

        [Given(@"a document candidate ""(.*)""")]
        public void GivenADocumentCandidate(string doc)
        {
            Try(() => _document = new Document(doc));
        }

        [When(@"the document candidate is materialized")]
        public void WhenTheDocumentCandidateIsMaterialized()
        {
            // validação já ocorreu no ctor
        }

        [Given(@"an email candidate ""(.*)""")]
        public void GivenAnEmailCandidate(string email)
        {
            Try(() => _email = new Email(email));
        }

        [When(@"the email candidate is materialized")]
        public void WhenTheEmailCandidateIsMaterialized()
        {
            // validação já ocorreu no ctor
        }

        [Given(@"a money candidate ""(.*)""")]
        public void GivenAMoneyCandidate(string amountText)
        {
            if (!decimal.TryParse(amountText, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount) &&
                !decimal.TryParse(amountText, NumberStyles.Number, CultureInfo.CurrentCulture, out amount))
            {
                amount = decimal.MinValue;
            }

            Try(() => _money = new Money(amount));
        }

        [When(@"the money candidate is materialized")]
        public void WhenTheMoneyCandidateIsMaterialized()
        {
            // validação já ocorreu no ctor
        }

        [Then(@"a vo domain error should mention ""(.*)""")]
        public void ThenVoDomainErrorShouldMention(string key)
        {
            _ex.Should().NotBeNull();
            MessageAlias.AssertMatches(_ex!.Message, key, AliasGroup.ValueObjects);
        }

        [Then(@"no vo domain error should occur")]
        public void ThenNoVoDomainErrorShouldOccur()
        {
            _ex.Should().BeNull();
        }


        private void Try(Action ctor)
        {
            try { ctor(); }
            catch (Exception e) { _ex = e; }
        }
    }
}
