namespace Neo4jJsonToCSharpClasses.Tests;

using FluentAssertions;

public class StringExtensionsTests
{
    public class ToUpperCamelCaseMethod
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ReturnsInput_WhenInputIsNullOrWhitespace(string input)
        {
            input.ToUpperCamelCase().Should().Be(input);
        }

        [Theory]
        [InlineData("testMethod", "TestMethod")]
        [InlineData("testMethodMultiple", "TestMethodMultiple")]
        [InlineData("TestMethod", "TestMethod")]
        [InlineData("TestMethodMultiple", "TestMethodMultiple")]
        [InlineData("MethodAB", "MethodAB")]
        [InlineData("MethodABC", "MethodAbc")]
        public void ReturnsExpectedUpperCasing(string input, string expected)
        {
            input.ToUpperCamelCase().Should().Be(expected);
        }

    }
}