using RX.Nyss.Common.Utils;
using Xunit;

namespace RX.Nyss.Common.Tests.Utils;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData("   ", null)]
    [InlineData("A", null)]
    [InlineData("1.1", null)]
    public void ParseToNullableInt(string value, int? expected) => Assert.Equal(expected, value.ParseToNullableInt());

    [Theory]
    [InlineData("1", 1, "1")]
    [InlineData("21", 1, "1")]
    [InlineData("321", 1, "1")]
    [InlineData("", 1, "")]
    [InlineData(null, 1, null)]
    public void SubstringFromEnd(string value, int numberOfCharacters, string expected) => Assert.Equal(expected, value.SubstringFromEnd(numberOfCharacters));

    [Theory]
    [InlineData("1", 1, "1")]
    [InlineData("124", 2, "12")]
    [InlineData("", 2, "")]
    [InlineData(null, 2, null)]
    public void Truncate(string value, int numberOfCharacters, string expected) => Assert.Equal(expected, value.Truncate(numberOfCharacters));
}
