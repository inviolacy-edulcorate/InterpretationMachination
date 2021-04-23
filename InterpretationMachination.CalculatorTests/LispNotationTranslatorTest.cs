using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.Calculator;
using Xunit;

namespace InterpretationMachination.CalculatorTests
{
    [ExcludeFromCodeCoverage]
    public class LispNotationTranslatorTest
    {
        [Fact]
        public void TestInterpretSimple()
        {
            // Arrange
            var subject = new LispNotationTranslator();

            // Act
            var result = subject.Interpret("2 + 3");

            // Assert
            Assert.Equal("(+ 2 3)", result);
        }

        [Fact]
        public void TestInterpretExample()
        {
            // Arrange
            var subject = new LispNotationTranslator();

            // Act
            var result = subject.Interpret("(2 + 3 * 5)");

            // Assert
            Assert.Equal("(+ 2 (* 3 5))", result);
        }

        [Fact]
        public void TestInterpretUnary()
        {
            // Arrange
            var subject = new LispNotationTranslator();

            // Act
            var result = subject.Interpret("-(2 + 3 * 5)");

            // Assert
            Assert.Equal("(- (+ 2 (* 3 5)))", result);
        }
    }
}