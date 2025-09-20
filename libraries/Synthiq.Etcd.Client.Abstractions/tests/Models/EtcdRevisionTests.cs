using System;
using Xunit;

namespace Synthiq.Etcd.Client.Tests;

[Trait("Category", "Unit")]
public sealed class EtcdRevisionTests
{
    [Fact]
    public void DefaultEqualsZero()
    {
        var d = default(EtcdRevision);

        Assert.Equal(EtcdRevision.Zero, d);
        Assert.Equal(0L, d.Value);
        Assert.True(d.IsZero);
        Assert.False(d.IsNonZero);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(Int64.MaxValue)]
    public void FromNonNegativeSucceeds(Int64 v)
    {
        var rev = EtcdRevision.From(v);

        Assert.Equal(v, rev.Value);
        Assert.Equal(v == 0L, rev.IsZero);
        Assert.Equal(v != 0L, rev.IsNonZero);
    }

    [Fact]
    public void FromNegativeThrows()
        => Assert.Throws<ArgumentOutOfRangeException>(()
            => EtcdRevision.From(-1L));

    [Fact]
    public void MaxValueIsInt64Max()
        => Assert.Equal(Int64.MaxValue, EtcdRevision.MaxValue.Value);

    [Fact]
    public void MinValueEqualsZero()
        => Assert.Equal(EtcdRevision.Zero, EtcdRevision.MinValue);

    [Fact]
    public void OperatorsWork()
    {
        var a = EtcdRevision.From(1L);
        var b = EtcdRevision.From(2L);

#pragma warning disable CS1718 // Comparison made to same variable
        Assert.True(a < b);
        Assert.True(a <= b);
        Assert.True(b > a);
        Assert.True(b >= a);
        Assert.True(a <= a);
        Assert.True(a >= a);
#pragma warning restore CS1718 // Comparison made to same variable
    }

    [Fact]
    public void IComparableGeneric()
    {
        var a = EtcdRevision.From(1L);
        var b = EtcdRevision.From(2L);

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(a));
    }

    [Fact]
    public void IComparableNonGeneric()
    {
        var a = EtcdRevision.From(1L);
        var b = EtcdRevision.From(2L);

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IComparable cmp = a;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        Assert.True(cmp.CompareTo(b) < 0);
        Assert.Equal(1, cmp.CompareTo(null));

        Assert.Throws<ArgumentException>(()
            => cmp.CompareTo("Definitely not an EtcdRevision"));
    }

    [Fact]
    public void SortingUsesCompareTo()
    {
        var arr = new[]
        {
            EtcdRevision.From(5L),
            EtcdRevision.Zero,
            EtcdRevision.From(2L)
        };

        Array.Sort(arr);

        Assert.Equal(
            [0L, 2L, 5L],
            [arr[0].Value, arr[1].Value, arr[2].Value]);
    }

    [Theory]
    [InlineData(0L, "0")]
    [InlineData(3L, "3")]
    [InlineData(Int64.MaxValue, "9223372036854775807")]
    public void ToStringFormatsCorrectly(Int64 v, String expected)
        => Assert.Equal(expected, EtcdRevision.From(v).ToString());
}
