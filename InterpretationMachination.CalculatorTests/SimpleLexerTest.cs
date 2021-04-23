using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.Calculator;
using InterpretationMachination.DataStructures.Tokens;
using Xunit;

namespace InterpretationMachination.CalculatorTests
{
    [ExcludeFromCodeCoverage]
    public class SimpleLexerTest
    {
        [Fact]
        public void TestInputString()
        {
            // Arrange
            var subject = new SimpleLexer();

            // Act
            subject.InputString("4+5");

            // Assert
            Assert.Equal("4+5", subject.Text);
            Assert.Equal(0, subject.Position);
            Assert.Equal(1, subject.NextPosition);
            Assert.Equal('4', subject.CurrentChar);
            Assert.Equal('+', subject.NextChar);
        }

        [Fact]
        public void TestGetNextTokenEmptyString()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.EndOfFile, result.Type);
        }

        [Fact]
        public void TestGetNextTokenOneWhitespaceString()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString(" ");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.EndOfFile, result.Type);
        }

        [Fact]
        public void TestGetNextTokenWhitespaceString()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("    ");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.EndOfFile, result.Type);
        }

        [Fact]
        public void TestGetNextTokenWhitespaceThenIntString()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("    45");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.Integer, result.Type);
            Assert.Equal(45, result.ValueAsInt);
        }

        [Fact]
        public void TestGetNextTokenIntOne()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("5");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.Integer, result.Type);
            Assert.Equal(5, result.ValueAsInt);
        }

        [Fact]
        public void TestGetNextTokenIntMany()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("5749");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.Integer, result.Type);
            Assert.Equal(5749, result.ValueAsInt);
        }

        [Fact]
        public void TestGetNextTokenIntToOp()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("5749*");
            subject.GetNextToken();

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.OpMul, result.Type);
        }

        [Fact]
        public void TestGetNextTokenIntManyWithFollowing()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("9856*");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.Integer, result.Type);
            Assert.Equal(9856, result.ValueAsInt);
        }

        [Fact]
        public void TestGetNextTokenOpMul()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("*");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.OpMul, result.Type);
        }

        [Fact]
        public void TestGetNextTokenOpDiv()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("/");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.OpDiv, result.Type);
        }

        [Fact]
        public void TestGetNextTokenOpAdd()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("+");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.OpAdd, result.Type);
        }

        [Fact]
        public void TestGetNextTokenOpSub()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("-");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.OpSub, result.Type);
        }

        [Fact]
        public void TestGetNextTokenOpenPar()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString("(");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.ParL, result.Type);
        }

        [Fact]
        public void TestGetNextTokenClosePar()
        {
            // Arrange
            var subject = new SimpleLexer();
            subject.InputString(")");

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TokenType.ParR, result.Type);
        }
    }
}