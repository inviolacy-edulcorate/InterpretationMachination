using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.Calculator;
using Xunit;

namespace InterpretationMachination.CalculatorTests
{
    [ExcludeFromCodeCoverage]
    public class SimpleCalculatorTest
    {
        [Fact]
        public void TestAddition()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("1+5");

            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public void TestAdditionMultipleLeft()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("10+5");

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void TestAdditionMultipleRight()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("3+45");

            // Assert
            Assert.Equal(48, result);
        }

        [Fact]
        public void TestAdditionWithSpaces()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("3 + 45");

            // Assert
            Assert.Equal(48, result);
        }

        [Fact]
        public void TestSubtractionToNegative()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("4-8");

            // Assert
            Assert.Equal(-4, result);
        }

        [Fact]
        public void TestSubtractionToPositive()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("9-3");

            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public void TestIntegerMoreThan2Digits()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("10076+420");

            // Assert
            Assert.Equal(10496, result);
        }

        [Fact]
        public void TestIntegerMultiplication()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("57 * 13");

            // Assert
            Assert.Equal(741, result);
        }

        [Fact]
        public void TestIntegerDivision()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("30 / 5");

            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public void TestIntegerMultiplicationDivision()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("57 * 13 / 54");

            // Assert
            Assert.Equal(13, result);
        }

        [Fact]
        public void TestIntegerMultiplicationDivisionMultiplication()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("57 * 13 / 54 * 10");

            // Assert
            Assert.Equal(130, result);
        }

        [Fact]
        public void TestIntegerMultiplicationDivisionMultiplicationNoWhitespace()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("57*13/54*10");

            // Assert
            Assert.Equal(130, result);
        }

        [Fact]
        public void TestAdditionMultiple()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("30 + 5 + 4 + 9 + 126");

            // Assert
            Assert.Equal(174, result);
        }

        [Fact]
        public void TestSubtractionMultiple()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("126 - 5 - 4 - 9");

            // Assert
            Assert.Equal(108, result);
        }

        [Fact]
        public void TestAdditionSubtractionMultiple()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("126 - 5 + 4 - 9");

            // Assert
            Assert.Equal(116, result);
        }

        [Fact]
        public void TestAddSubMulDiv1()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("2 + 7 * 4");

            // Assert
            Assert.Equal(30, result); // No order of operations.
        }

        [Fact]
        public void TestAddSubMulDiv2()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("7 - 8 / 4");

            // Assert
            Assert.Equal(5, result); // No order of operations.
        }

        [Fact]
        public void TestAddSubMulDiv3()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("14 + 2 * 3 - 6 / 2");

            // Assert
            Assert.Equal(17, result); // No order of operations.
        }

        [Fact]
        public void TestNestedExprNotNested()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("(5 + 4 * 3)");

            // Assert
            Assert.Equal(17, result); // No order of operations.
        }

        [Fact]
        public void TestNestedExprNestedStart()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("(5 + 4) * 3");

            // Assert
            Assert.Equal(27, result); // No order of operations.
        }

        [Fact]
        public void TestNestedExprNestedMiddle()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("4 * (5 + 4) * 3");

            // Assert
            Assert.Equal(108, result); // No order of operations.
        }

        [Fact]
        public void TestNestedExprNestedEnd()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("3 * (3 + 8)");

            // Assert
            Assert.Equal(33, result); // No order of operations.
        }

        [Fact]
        public void TestNestedExprNestedDeep()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("3 * ((3+7) * (15-5))");

            // Assert
            Assert.Equal(300, result); // No order of operations.
        }

        [Fact]
        public void TestUnaryOpAddSub()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("+-3");

            // Assert
            Assert.Equal(-3, result); // No order of operations.
        }

        [Fact]
        public void TestUnaryOpSubSub()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("5--3");

            // Assert
            Assert.Equal(8, result); // No order of operations.
        }

        [Fact]
        public void TestUnaryOpNumAddSub()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("5+-2");

            // Assert
            Assert.Equal(3, result); // No order of operations.
        }

        [Fact]
        public void TestUnaryOpSubSubSub()
        {
            // Arrange
            var subject = new SimpleCalculator();

            // Act
            var result = subject.Interpret("5---2");

            // Assert
            Assert.Equal(3, result); // No order of operations.
        }
    }
}