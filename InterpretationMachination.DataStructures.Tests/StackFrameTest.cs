using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.DataStructures.CallStack;
using InterpretationMachination.DataStructures.SymbolTable;
using Xunit;

namespace InterpretationMachination.DataStructures.Tests
{
    [ExcludeFromCodeCoverage]
    public class StackFrameTest
    {
        [Fact]
        public void TestConstructor()
        {
            // Arrange


            // Act
            var subject = new StackFrame(CreateScopedSymbolTable(), "GLOBAL", 1);

            // Assert
            Assert.Equal(3, subject.Data.Count);
        }

        [Fact]
        public void TestIndexSetGet()
        {
            // Arrange
            var subject = new StackFrame(CreateScopedSymbolTable(), "GLOBAL", 1);

            // Act
            subject["INTEGER"] = 14;

            // Assert
            Assert.Equal(14, subject["INTEGER"]);
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