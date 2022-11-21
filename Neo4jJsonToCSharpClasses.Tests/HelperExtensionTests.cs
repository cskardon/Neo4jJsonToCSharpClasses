namespace Neo4jJsonToCSharpClasses.Tests;

using FluentAssertions;

public class HelperExtensionsTests
{
    public class ToCSharpTypeMethod
    {
        [Theory]
        [InlineData("String", "string")]
        [InlineData("Integer","int")]
        [InlineData("Float","float")]
        [InlineData("Boolean","bool")]
        [InlineData("Date","DateTime")]
        [InlineData("DateTime","DateTime")]
        [InlineData("StringArray", "string[]")]
        [InlineData("IntegerArray","int[]")]
        [InlineData("FloatArray","float[]")]
        [InlineData("BooleanArray","bool[]")]
        [InlineData("DateArray","DateTime[]")]
        [InlineData("DateTimeArray","DateTime[]")]
        public void ConvertsTypesCorrectly(string javaType, string expected)
        {
            javaType.ToCSharpType().Should().Be(expected);
        }

        [Theory]
        [InlineData("STRING", "string")]
        [InlineData("string", "string")]
        [InlineData("STRing", "string")]
        [InlineData("strING", "string")]
        public void DoesNotCareAboutCase(string javaType, string expected)
        {
            javaType.ToCSharpType().Should().Be(expected);
        }

        [Fact]
        public void ThrowsArgumentOutOfRangeException_WhenJavaTypeIsNotKnown()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "unknownType".ToCSharpType());
        }
    }
}