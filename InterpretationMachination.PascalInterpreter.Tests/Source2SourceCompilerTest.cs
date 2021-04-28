using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace InterpretationMachination.PascalInterpreter.Tests
{
    [ExcludeFromCodeCoverage]
    public class Source2SourceCompilerTest
    {
        [Fact]
        public void TestVisitTree()
        {
            // Arrange
            var subject = new Source2SourceCompiler();

            // Act
            subject.Compile(@"program Main;" + Environment.NewLine + @"   var b, x, y : real;" + Environment.NewLine +
                            @"   var z : integer;" + Environment.NewLine + @"" + Environment.NewLine +
                            @"   procedure AlphaA(a : integer);" + Environment.NewLine + @"      var b : integer;" +
                            Environment.NewLine + @"" + Environment.NewLine + @"      procedure Beta(c : integer);" +
                            Environment.NewLine + @"         var y : integer;" + Environment.NewLine + @"" +
                            Environment.NewLine + @"         procedure Gamma(c : integer);" + Environment.NewLine +
                            @"            var x : integer;" + Environment.NewLine + @"         begin { Gamma }" +
                            Environment.NewLine + @"            x := a + b - c * x / y div z;" + Environment.NewLine +
                            @"         end;  { Gamma }" + Environment.NewLine + @"" + Environment.NewLine +
                            @"      begin { Beta }" + Environment.NewLine + @"" + Environment.NewLine +
                            @"      end;  { Beta }" + Environment.NewLine + @"" + Environment.NewLine +
                            @"   begin { AlphaA }" + Environment.NewLine + @"" + Environment.NewLine +
                            @"   end;  { AlphaA }" + Environment.NewLine + @"" + Environment.NewLine +
                            @"   procedure AlphaB(a : integer);" + Environment.NewLine + @"      var c : real;" +
                            Environment.NewLine + @"   begin { AlphaB }" + Environment.NewLine + @"      c := a + b;" +
                            Environment.NewLine + @"   end;  { AlphaB }" + Environment.NewLine + @"" +
                            Environment.NewLine + @"begin { Main }" + Environment.NewLine + @"end.  { Main }");

            // Assert
            Assert.Equal(
                @"program Main0;" + Environment.NewLine + @"   var b1 : REAL0;" + Environment.NewLine +
                @"   var x1 : REAL0;" + Environment.NewLine + @"   var y1 : REAL0;" + Environment.NewLine +
                @"   var z1 : INTEGER0;" + Environment.NewLine + @"   procedure AlphaA1(a2 : INTEGER0);" +
                Environment.NewLine + @"      var b2 : INTEGER0;" + Environment.NewLine +
                @"      procedure Beta2(c3 : INTEGER0);" + Environment.NewLine + @"         var y3 : INTEGER0;" +
                Environment.NewLine + @"         procedure Gamma3(c4 : INTEGER0);" + Environment.NewLine +
                @"            var x4 : INTEGER0;" + Environment.NewLine + @"" + Environment.NewLine +
                @"         begin" + Environment.NewLine +
                @"            <x4:INTEGER> := <a2:INTEGER> + <b2:INTEGER> - <c4:INTEGER> * <x4:INTEGER> / <y3:INTEGER> div <z1:INTEGER>;" +
                Environment.NewLine + @"         end; {END OF Gamma}" + Environment.NewLine + @"" +
                Environment.NewLine + @"      begin" + Environment.NewLine + @"" + Environment.NewLine +
                @"      end; {END OF Beta}" + Environment.NewLine + @"" + Environment.NewLine + @"   begin" +
                Environment.NewLine + @"" + Environment.NewLine + @"   end; {END OF AlphaA}" + Environment.NewLine +
                @"   procedure AlphaB1(a2 : INTEGER0);" + Environment.NewLine + @"      var c2 : REAL0;" +
                Environment.NewLine + @"" + Environment.NewLine + @"   begin" + Environment.NewLine +
                @"      <c2:REAL> := <a2:INTEGER> + <b1:REAL>;" + Environment.NewLine + @"   end; {END OF AlphaB}" +
                Environment.NewLine + @"" + Environment.NewLine + @"begin" + Environment.NewLine + @"" +
                Environment.NewLine + @"end. {END OF Main}", subject.Output);
        }
    }
}