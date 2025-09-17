using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace ContractingService.Tests.Unit.Shared.Assertions
{
    public enum AliasGroup { Coverage, Insured, ValueObjects, Contract, Application }

    public static class MessageAlias
    {
        private static readonly Dictionary<AliasGroup, Dictionary<string, IEnumerable<string>>> Map =
            new()
            {
                [AliasGroup.Coverage] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["coverage name"] = new[] { "Coverage name", "cannot be empty" },
                    ["premium"] = new[] { "Premium", "invalid", "negative", "must" },
                    ["only in draft"] = new[] { "only be added while contract is in Draft", "in Draft status" },
                },
                [AliasGroup.Insured] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["insured name"] = new[] { "Insured", "name", "cannot be empty", "is required" },
                    ["document"] = new[] { "Document", "required", "invalid", "length", "digits" },
                    ["email"] = new[] { "Email", "invalid", "format", "required" },
                },
                [AliasGroup.ValueObjects] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["document empty"] = new[] { "Document", "required", "empty", "cannot be empty" },
                    ["document length"] = new[] { "Document", "length", "digits", "must have" },
                    ["document format"] = new[] { "Document", "invalid", "format", "digits only" },
                    ["email empty"] = new[] { "Email", "required", "empty" },
                    ["email format"] = new[] { "Email", "invalid", "format" },
                    ["money negative"] = new[] { "Money", "Amount", "negative", "non-negative", ">= 0", "must be" },
                    ["money zero"] = new[] { "Money", "Amount", "zero", "must be greater than 0" },
                    ["money scale"] = new[] { "Money", "scale", "decimal places", "precision" },
                    ["document checksum"] = new[] { "Document", "checksum", "check digit", "invalid" },
                },
                [AliasGroup.Contract] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["only active"] = new[] { "Only active", "only active contracts", "active contracts can be canceled" },
                    ["duplicate"] = new[] { "duplicate", "already exists" },
                    ["final state"] = new[] { "Canceled", "Terminated", "final state", "cannot add coverage" },
                    ["without coverages"] = new[] { "at least one coverage", "without coverages" },
                },
                [AliasGroup.Application] = new(StringComparer.OrdinalIgnoreCase)

            };

        public static bool Matches(string? actual, string requested, AliasGroup group)
        {
            if (string.IsNullOrEmpty(actual)) return false;

            var candidates = new List<string> { requested };
            if (Map.TryGetValue(group, out var dict) &&
                dict.TryGetValue(requested, out var mapped))
            {
                candidates.AddRange(mapped);
            }

            return candidates.Any(c => actual.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static bool Matches(string? actual, string requested)
        {
            if (string.IsNullOrEmpty(actual)) return false;
            return actual.IndexOf(requested, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool Matches(string? actual, string requested,
            Dictionary<string, IEnumerable<string>> aliases)
        {
            if (string.IsNullOrEmpty(actual)) return false;

            var candidates = new List<string> { requested };
            if (aliases.TryGetValue(requested, out var mapped)) candidates.AddRange(mapped);

            return candidates.Any(c => actual.IndexOf(c, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static void AssertMatches(string? actual, string requested, AliasGroup group)
        {
            Matches(actual, requested, group).Should().BeTrue(
                $"expected '{actual}' to mention '{requested}' via aliases ({group})");
        }
    }
}
