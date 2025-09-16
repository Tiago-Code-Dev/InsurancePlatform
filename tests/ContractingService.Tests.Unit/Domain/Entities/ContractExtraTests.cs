using ContractingService.Domain.Entities;
using ContractingService.Domain.Enums;
using ContractingService.Domain.ValueObjects;
using FluentAssertions;

public sealed class ContractExtraTests
{
    private static Contract NewDraft() =>
        new(new Insured("John", new Document("12345678901"), new Email("john@doe.com"), Guid.NewGuid()));

    private static Coverage Cov(decimal p = 100m) =>
        new("Basic", CoverageType.Basic, new Money(p));

    [Fact]
    public void AddCoverage_AfterCanceled_ShouldThrow()
    {
        var c = NewDraft(); c.AddCoverage(Cov()); c.Activate(); c.Cancel();
        Action act = () => c.AddCoverage(Cov(50));
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void AddCoverage_AfterTerminated_ShouldThrow()
    {
        var c = NewDraft(); c.Terminate();
        Action act = () => c.AddCoverage(Cov(50));
        act.Should().Throw<Exception>(); 
    }

    [Fact]
    public void Activate_Twice_ShouldBeIdempotent_AndNotThrow()
    {
        var c = NewDraft(); c.AddCoverage(Cov()); c.Activate();
        var first = c.ActivatedAt;

        Action second = () => c.Activate();

        second.Should().NotThrow();                          
        c.Status.ToString().Should().Be("Active");         
        c.ActivatedAt.Should().NotBeNull();
        c.ActivatedAt!.Value.Should().BeOnOrAfter((DateTime)first!);  
    }

    [Fact]
    public void AddSameCoverage_Twice_ShouldAllowAndKeepBoth()
    {
        var c = NewDraft();
        var first = Cov(100);
        c.AddCoverage(first);

        Action act = () => c.AddCoverage(new Coverage(first.Name, first.Type, new Money(100)));

        act.Should().NotThrow();                         
        c.Coverages.Should().HaveCount(2);                  
        c.Coverages.Count(x => x.Name == first.Name
                            && x.Type == first.Type).Should().Be(2);
    }

}

