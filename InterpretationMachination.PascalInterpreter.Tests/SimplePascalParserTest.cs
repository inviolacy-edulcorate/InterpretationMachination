using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.Interfaces.Exceptions;
using Xunit;

namespace InterpretationMachination.PascalInterpreter.Tests
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class SimplePascalParserTest
    {
        [Fact]
        public void TestParse()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            subject.Parse(@"
                PROGRAM Part10;
                BEGIN
                    BEGIN
                        number := 2;
                        a := number;
                        b := 10 * a + 10 * number div 4;
                        c := a - - b
                    END;
                    x := 11;
                END.
                ");

            // Assert
        }

        [Fact]
        public void TestParseNumericVariable()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                PROGRAM Part10;
                BEGIN
                    a12 := 23;
                END.
                ");
        }

        [Fact]
        public void TestParseV10()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                PROGRAM Part10;
                VAR
                    number     : INTEGER;
                    a, b, c, x : INTEGER;
                    y          : REAL;
                            
                    BEGIN {Part10
                    }
                    BEGIN
                        number := 2;
                    a := number;
                    b := 10 * a + 10 * number DIV 4;
                    c := a - - b
                        END;
                    x := 11;
                    y := 20 / 7 + 3.14;
                    { writeln('a = ', a);
                    }
                    { writeln('b = ', b); }
                    { writeln('c = ', c); }
                    { writeln('number = ', number); }
                    { writeln('x = ', x); }
                    { writeln('y = ', y); }
                    END.  { Part10}
                ");
        }

        [Fact]
        public void TestParseV12_1()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                PROGRAM Part12;
                VAR
                   a : INTEGER;
                
                PROCEDURE P1;
                VAR
                   a : REAL;
                   k : INTEGER;
                
                   PROCEDURE P2;
                   VAR
                      a, z : INTEGER;
                   BEGIN {P2}
                      z := 777;
                   END;  {P2}
                
                BEGIN {P1}
                
                END;  {P1}
                
                BEGIN {Part12}
                   a := 10;
                END.  {Part12}
                ");
        }

        [Fact]
        public void TestParseV12_2()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                PROGRAM Test;
                VAR
                   a : INTEGER;
                
                PROCEDURE P1;
                BEGIN {P1}
                
                END;  {P1}
                
                PROCEDURE P1A;
                BEGIN {P1A}
                
                END;  {P1A}
                
                BEGIN {Test}
                   a := 10;
                END.  {Test}
                ");
        }

        [Fact]
        public void TestParseProcedureWithParameter()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var x, y: real;
                
                   procedure Alpha(a : integer);
                      var y : integer;
                   begin
                      x := a + x + y;
                   end;
                
                begin { Main }
                
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseProcedureWithParametersDifferentType()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                {Made by me}
                program Main;
                   var x, y: real;
                
                   procedure Alpha(a : integer; b : real);
                      var y : integer;
                   begin
                      x := a + x + y;
                   end;
                
                begin { Main }
                
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseProcedureWithParametersDifferentTypeMultiple()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                {Made by me}
                program Main;
                   var x, y: real;
                
                   procedure Alpha(a : integer; b, c : real);
                      var y : integer;
                   begin
                      x := a + x + y;
                   end;
                
                begin { Main }
                
                end.  { Main }
                ");
        }


        [Fact]
        public void TestParseMultipleProcedures()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var x, y : real;
                
                   procedure AlphaA(a : integer);
                      var y : integer;
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                   procedure AlphaB(a : integer);
                      var b : integer;
                   begin { AlphaB }
                
                   end;  { AlphaB }
                
                begin { Main }
                
                end.  { Main }
                ");
        }


        [Fact]
        public void TestParseSemanticError()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var x, y: real;
                
                   procedure Alpha(a : integer);
                      var y : integer;
                   begin
                      x := b + x + y; { ERROR here! }
                   end;
                
                begin { Main }
                
                end.  { Main }
                ");
        }


        [Fact]
        public void TestParseComplexProcedures()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer);
                      var b : integer;
                
                      procedure Beta(c : integer);
                         var y : integer;
                
                         procedure Gamma(c : integer);
                            var x : integer;
                         begin { Gamma }
                            x := a + b + c + x + y + z;
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
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseException()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = Assert.Throws<UnexpectedTokenTypeException>(() => subject.Parse(@"
                program ;  
                "));

            // Assert
            Assert.Equal(PascalTokenType.Id, result.ExpectedTokenType);
            Assert.Equal(PascalTokenType.SemCol, result.FoundTokenType);
        }

        [Fact]
        public void TestParseProcedureCall()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer);
                      var b : integer;
                
                      procedure Beta(c : integer);
                         var y : integer;
                
                         procedure Gamma(c : integer);
                            var x : integer;
                         begin { Gamma }
                            x := a + b + c + x + y + z;
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
                    AlphaA(3);
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseProcedureCallExprParam()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer);
                      var b : integer;
                
                      procedure Beta(c : integer);
                         var y : integer;
                
                         procedure Gamma(c : integer);
                            var x : integer;
                         begin { Gamma }
                            x := a + b + c + x + y + z;
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
                    AlphaA(3 + 6);
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseProcedureCallMultiParam()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer; c : real);
                      var b : integer;
                
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                   procedure AlphaB(a : integer);
                      var c : real;
                   begin { AlphaB }
                      c := a + b;
                   end;  { AlphaB }
                
                begin { Main }
                    AlphaA(3, 34);
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseProcedureCallMultiParamExprParam()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer; c : real);
                      var b : integer;
                
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                   procedure AlphaB(a : integer);
                      var c : real;
                   begin { AlphaB }
                      c := a + b;
                   end;  { AlphaB }
                
                begin { Main }
                    AlphaA(3 + 6, 34);
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseProcedureCallNoParams()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA;
                      var b : integer;
                
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                   procedure AlphaB(a : integer);
                      var c : real;
                   begin { AlphaB }
                      c := a + b;
                   end;  { AlphaB }
                
                begin { Main }
                    AlphaA();
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseString()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var str : string;
                
                
                begin { Main }
                    str := 'Hello World!';
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseIfThen()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var str : string;
                begin { Main }
                    if ( 1 = 1 ) then
                        str := 'Hello World!';
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseIfThenElse()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var str : string;
                begin { Main }
                    if ( 1 = 1 ) then
                        str := 'Hello World!'
                    else
                        str := 'Bye World!';
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseStringIndex()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var s : string;
                begin { Main }
                    s := 'Hello World!';
                    s := s[0];
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseStringWhileDo()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var i,j : integer;
                   var l : boolean;
                begin { Main }
                    i:=0;
                    j:=13;
                    l := true;
                    while l do
                    begin
                       i := i + 1;
                       if (i-j=0) then
                          l := false;
                    end;
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseStringHashtagStringLiteral()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var i : string;
                begin { Main }
                    i := #10;
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseArrayDeclarationIntToInt()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var n: array [1..10] of integer;
                begin { Main }
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseArrayDeclarationInteger()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var n: array [integer] of integer;
                begin { Main }
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseSubRangeType()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var n: 1..10;
                begin { Main }
                end.  { Main }
                ");
        }

        [Fact]
        public void TestParseArrayDeclarationArray()
        {
            // Arrange
            var subject = new SimplePascalParser();

            // Act
            var result = subject.Parse(@"
                program Main;
                   var n: array [integer] of array [integer] of integer;
                begin { Main }
                end.  { Main }
                ");
        }
    }
}