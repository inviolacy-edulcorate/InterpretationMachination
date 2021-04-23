using System;
using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.DataStructures.CallStack;
using InterpretationMachination.DataStructures.SymbolTable;
using Xunit;

namespace InterpretationMachination.DataStructures.Tests
{
    [ExcludeFromCodeCoverage]
    public class BasicCallStackTest
    {
        [Fact]
        public void TestStackEmptyOnCreate()
        {
            // Arrange
            var subject = new BasicCallStack();

            // Act
            Assert.Throws<InvalidOperationException>(() => subject.Top);

            // Assert
        }

        [Fact]
        public void TestPush()
        {
            // Arrange
            var subject = new BasicCallStack();
            var stackFrame = new StackFrame(CreateScopedSymbolTable(), "PROGRAM", 1);

            // Act
            subject.Push(stackFrame);

            // Assert
            Assert.Same(stackFrame, subject.Top);
        }

        [Fact]
        public void TestPush2()
        {
            // Arrange
            var subject = new BasicCallStack();
            var stackFrame = new StackFrame(CreateScopedSymbolTable(), "PROGRAM", 1);
            var stackFrame2 = new StackFrame(CreateScopedSymbolTable(), "PROCEDURE", 2);
            subject.Push(stackFrame);

            // Act
            subject.Push(stackFrame2);

            // Assert
            Assert.Same(stackFrame2, subject.Top);
        }

        [Fact]
        public void TestPop()
        {
            // Arrange
            var subject = new BasicCallStack();
            var stackFrame = new StackFrame(CreateScopedSymbolTable(), "PROGRAM", 1);
            var stackFrame2 = new StackFrame(CreateScopedSymbolTable(), "PROCEDURE", 2);
            subject.Push(stackFrame);
            subject.Push(stackFrame2);

            // Act
            var result = subject.Pop();

            // Assert
            Assert.Same(stackFrame2, result);
            Assert.Same(stackFrame, subject.Top);
        }

        private ScopedSymbolTable CreateScopedSymbolTable()
        {
            var sst = new ScopedSymbolTable();

            sst.DeclareSymbol(new BuiltinSymbol("INTEGER"));
            sst.DeclareSymbol(new BuiltinSymbol("REAL"));
            sst.DeclareSymbol(new BuiltinSymbol("BOOLEAN"));

            return sst;
        }
    }
}