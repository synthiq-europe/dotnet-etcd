using System;
using Xunit;

namespace Synthiq.Etcd.Client.Tests;

[Trait("Category", "Unit")]
public sealed class EtcdWatchIdTests
{
    [Fact]
    public void DefaultEqualsNone()
    {
        var d = default(EtcdWatchId);

        Assert.Equal(EtcdWatchId.None, d);
        Assert.Equal(0L, d.Value);
        Assert.True(d.IsNone);
        Assert.False(d.IsSome);
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(3L)]
    [InlineData(Int64.MaxValue)]
    public void FromPositiveSucceeds(Int64 v)
    {
        var id = EtcdWatchId.From(v);

        Assert.Equal(v, id.Value);
        Assert.False(id.IsNone);
        Assert.True(id.IsSome);
    }

    [Fact]
    public void FromZeroThrows()
        => Assert.Throws<ArgumentOutOfRangeException>(()
            => EtcdWatchId.From(0L));

    [Fact]
    public void OperatorsWork()
    {
        var a = EtcdWatchId.From(1L);
        var b = EtcdWatchId.From(2L);

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
        var a = EtcdWatchId.From(1L);
        var b = EtcdWatchId.From(2L);

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(a));
    }

    [Fact]
    public void IComparableNonGeneric()
    {
        var a = EtcdWatchId.From(1L);
        var b = EtcdWatchId.From(2L);

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IComparable cmp = a;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        Assert.True(cmp.CompareTo(b) < 0);
        Assert.Equal(1, cmp.CompareTo(null));

        Assert.Throws<ArgumentException>(()
            => cmp.CompareTo("Definitely not an EtcdWatchId"));
    }

    [Fact]
    public void SortingUsesCompareTo()
    {
        var arr = new[]
        {
            EtcdWatchId.From(5L),
            EtcdWatchId.None,
            EtcdWatchId.From(2L)
        };

        Array.Sort(arr);

        Assert.Equal(
            [0L, 2L, 5L],
            [arr[0].Value, arr[1].Value, arr[2].Value]);
    }

    [Theory]
    [InlineData(0L, "None")]
    [InlineData(3L, "3")]
    [InlineData(Int64.MaxValue, "9223372036854775807")]
    public void ToStringFormatsCorrectly(Int64 v, String expected)
    {
        var id = v > 0L ? EtcdWatchId.From(v) : EtcdWatchId.None;

        Assert.Equal(expected, id.ToString());
    }
}
