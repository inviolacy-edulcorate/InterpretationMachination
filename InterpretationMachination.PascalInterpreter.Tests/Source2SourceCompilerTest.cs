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
            subject.Compile(@"program Main;
   var b, x, y : real;
   var z : integer;

   procedure AlphaA(a : integer);
      var b : integer;

      procedure Beta(c : integer);
         var y : integer;

         procedure Gamma(c : integer);
            var x : integer;
         begin { Gamma }
            x := a + b - c * x / y div z;
         end;  { Gamma }

      begin { Beta }

      end;  { Beta }

   begin { AlphaA }

   end;  { AlphaA }

   procedure AlphaB(a : integer);
      var c : real;
   begin { AlphaB }
      c := a + b;
   end;  { AlphaB }

begin { Main }
end.  { Main }");
            
            // Assert
            Assert.Equal(@"program Main0;
   var b1 : REAL0;
   var x1 : REAL0;
   var y1 : REAL0;
   var z1 : INTEGER0;
   procedure AlphaA1(a2 : INTEGER0);
      var b2 : INTEGER0;
      procedure Beta2(c3 : INTEGER0);
         var y3 : INTEGER0;
         procedure Gamma3(c4 : INTEGER0);
            var x4 : INTEGER0;

         begin
            <x4:INTEGER> := <a2:INTEGER> + <b2:INTEGER> - <c4:INTEGER> * <x4:INTEGER> / <y3:INTEGER> div <z1:INTEGER>;
         end; {END OF Gamma}

      begin

      end; {END OF Beta}

   begin

   end; {END OF AlphaA}
   procedure AlphaB1(a2 : INTEGER0);
      var c2 : REAL0;

   begin
      <c2:REAL> := <a2:INTEGER> + <b1:REAL>;
   end; {END OF AlphaB}

begin

end. {END OF Main}", subject.Output);
        }
    }
}