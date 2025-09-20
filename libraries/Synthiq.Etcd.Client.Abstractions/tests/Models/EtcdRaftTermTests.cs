using System;
using Xunit;

namespace Synthiq.Etcd.Client.Tests;

[Trait("Category", "Unit")]
public sealed class EtcdRaftTermTests
{
    [Fact]
    public void DefaultIsZero()
    {
        var d = default(EtcdRaftTerm);

        Assert.True(d.IsZero);
        Assert.False(d.IsNonZero);
        Assert.Equal(0UL, d.Value);
        Assert.Equal(EtcdRaftTerm.Zero, d);
    }

    [Theory]
    [InlineData(0UL)]
    [InlineData(1UL)]
    [InlineData(42UL)]
    [InlineData(UInt64.MaxValue)]
    public void FromUInt64Succeeds(UInt64 v)
    {
        var term = EtcdRaftTerm.From(v);

        Assert.Equal(v, term.Value);
        Assert.Equal(v == 0, term.IsZero);
        Assert.Equal(v != 0, term.IsNonZero);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(Int64.MaxValue)]
    public void FromInt64NonNegativeSucceeds(Int64 v)
    {
        var term = EtcdRaftTerm.From(v);

        Assert.Equal(v, (Int64)term.Value);
        Assert.Equal(v == 0, term.IsZero);
        Assert.Equal(v != 0, term.IsNonZero);
    }

    [Fact]
    public void FromInt64NegativeThrows()
        => Assert.Throws<ArgumentOutOfRangeException>(()
            => EtcdRaftTerm.From(-1L));

    [Fact]
    public void MaxValueIsUInt64Max()
        => Assert.Equal(UInt64.MaxValue, EtcdRaftTerm.MaxValue.Value);

    [Fact]
    public void MinValueEqualsZero()
        => Assert.Equal(EtcdRaftTerm.Zero, EtcdRaftTerm.MinValue);

    [Fact]
    public void OperatorsWork()
    {
        var a = EtcdRaftTerm.From(0L);
        var b = EtcdRaftTerm.From(1UL);
        var c = EtcdRaftTerm.From(2L);
        var d = EtcdRaftTerm.From(3UL);

#pragma warning disable CS1718 // Comparison made to same variable
        Assert.True(a < b);
        Assert.True(b < c);
        Assert.True(c < d);

        Assert.True(a <= b);
        Assert.True(b <= d);
        Assert.True(c <= d);

        Assert.True(b > a);
        Assert.True(c > b);
        Assert.True(d > c);

        Assert.True(b >= a);
        Assert.True(c >= b);
        Assert.True(d >= c);

        Assert.True(a <= a);
        Assert.True(b <= b);
        Assert.True(c <= c);
        Assert.True(d <= d);

        Assert.True(a >= a);
        Assert.True(b >= b);
        Assert.True(c >= c);
        Assert.True(d >= d);
#pragma warning restore CS1718 // Comparison made to same variable
    }

    [Fact]
    public void IComparableGeneric()
    {
        var a = EtcdRaftTerm.From(1UL);
        var b = EtcdRaftTerm.From(2UL);

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(a));
    }

    [Fact]
    public void IComparableNonGeneric()
    {
        var a = EtcdRaftTerm.From(1UL);
        var b = EtcdRaftTerm.From(2UL);

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IComparable cmp = a;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        Assert.True(cmp.CompareTo(b) < 0);
        Assert.Equal(1, cmp.CompareTo(null));

        Assert.Throws<ArgumentException>(()
            => cmp.CompareTo("Definitely not an EtcdRaftTerm"));
    }

    [Fact]
    public void SortingUsesCompareTo()
    {
        var arr = new[]
        {
            EtcdRaftTerm.From(5UL),
            EtcdRaftTerm.Zero,
            EtcdRaftTerm.From(2L)
        };

        Array.Sort(arr);

        Assert.Equal(
            [0UL, 2UL, 5UL],
            [arr[0].Value, arr[1].Value, arr[2].Value]);
    }

    [Theory]
    [InlineData(0L, "0")]
    [InlineData(3L, "3")]
    [InlineData(UInt64.MaxValue, "18446744073709551615")]
    public void ToStringFormatsCorrectly(UInt64 v, String expected)
        => Assert.Equal(expected, EtcdRaftTerm.From(v).ToString());
}
