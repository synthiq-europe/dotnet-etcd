using System;
using Xunit;

namespace Synthiq.Etcd.Client.Tests;

[Trait("Category", "Unit")]
public sealed class EtcdClusterIdTests
{
    [Fact]
    public void DefaultEqualsNone()
    {
        var d = default(EtcdClusterId);

        Assert.Equal(EtcdClusterId.None, d);
        Assert.Equal(0UL, d.Value);
        Assert.True(d.IsNone);
        Assert.False(d.IsSome);
    }

    [Theory]
    [InlineData(1UL)]
    [InlineData(3UL)]
    [InlineData(UInt64.MaxValue)]
    public void FromPositiveSucceeds(UInt64 v)
    {
        var id = EtcdClusterId.From(v);

        Assert.Equal(v, id.Value);
        Assert.False(id.IsNone);
        Assert.True(id.IsSome);
    }

    [Fact]
    public void FromZeroThrows()
        => Assert.Throws<ArgumentOutOfRangeException>(()
            => EtcdClusterId.From(0UL));

    [Fact]
    public void OperatorsWork()
    {
        var a = EtcdClusterId.From(1UL);
        var b = EtcdClusterId.From(2UL);

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
        var a = EtcdClusterId.From(1UL);
        var b = EtcdClusterId.From(2UL);

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(a));
    }

    [Fact]
    public void IComparableNonGeneric()
    {
        var a = EtcdClusterId.From(1UL);
        var b = EtcdClusterId.From(2UL);

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IComparable cmp = a;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        Assert.True(cmp.CompareTo(b) < 0);
        Assert.Equal(1, cmp.CompareTo(null));

        Assert.Throws<ArgumentException>(()
            => cmp.CompareTo("Definitely not an EtcdClusterId"));
    }

    [Fact]
    public void SortingUsesCompareTo()
    {
        var arr = new[]
        {
            EtcdClusterId.From(5UL),
            EtcdClusterId.None,
            EtcdClusterId.From(2UL)
        };

        Array.Sort(arr);

        Assert.Equal(
            [0UL, 2UL, 5UL],
            [arr[0].Value, arr[1].Value, arr[2].Value]);
    }

    [Theory]
    [InlineData(0UL, "None")]
    [InlineData(3uL, "0000000000000003")]
    [InlineData(UInt64.MaxValue, "ffffffffffffffff")]
    public void ToStringFormatsCorrectly(UInt64 v, String expected)
    {
        var id = v > 0UL ? EtcdClusterId.From(v) : EtcdClusterId.None;

        Assert.Equal(expected, id.ToString());
    }
}
