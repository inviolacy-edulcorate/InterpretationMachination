using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.Calculator;
using Xunit;

namespace InterpretationMachination.CalculatorTests
{
    [ExcludeFromCodeCoverage]
    public class ReversePolishNotationTranslatorTest
    {
        [Fact]
        public void TestInterpretSimple()
        {
            // Arrange
            var subject = new ReversePolishNotationTranslator();

            // Act
            var result = subject.Interpret("1+5");

            // Assert
            Assert.Equal("1 5 +", result);
        }

        [Fact]
        public void TestInterpretExample()
        {
            // Arrange
            var subject = new ReversePolishNotationTranslator();

            // Act
            var result = subject.Interpret("(5 + 3) * 12 / 3");

            // Assert
            Assert.Equal("5 3 + 12 * 3 /", result);
        }

        /// <summary>
        /// https://stackoverflow.com/a/64868283/10717433
        /// </summary>
        [Fact]
        public void TestInterpretUnary()
        {
            // Arrange
            var subject = new ReversePolishNotationTranslator();

            // Act
            var result = subject.Interpret("-(5 + 3) * 12 / 3");

            // Assert
            Assert.Equal("5 3 + NEGATE 12 * 3 /", result);
        }
    }
}