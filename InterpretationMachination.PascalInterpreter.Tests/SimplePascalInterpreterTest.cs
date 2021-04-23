using System;
using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.Interfaces.Exceptions;
using Xunit;

namespace InterpretationMachination.PascalInterpreter.Tests
{
    [ExcludeFromCodeCoverage]
    public class SimplePascalInterpreterTest
    {
        [Fact]
        public void TestInterpret()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                PROGRAM Part10;
                VAR
                    number, a, b, c, x : integer;
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
            Assert.Equal(2, subject.GlobalScope.Top["a"]);
            Assert.Equal(11, subject.GlobalScope.Top["x"]);
            Assert.Equal(27d, subject.GlobalScope.Top["c"]);
            Assert.Equal(25d, subject.GlobalScope.Top["b"]);
            Assert.Equal(2, subject.GlobalScope.Top["number"]);
        }

        [Fact]
        public void TestInterpretCaseInsensitive()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                PROGRAM Part10;
                VAR
                    number, a, b, c, x : integer;
                BEGIN
                    BEGIN
                        number := 2;
                        a := NumBer;
                        B := 10 * a + 10 * NUMBER div 4;
                        c := a - - b
                    end;

                    x := 11;
                END.
                ");

            // Assert
            Assert.Equal(2, subject.GlobalScope.Top["a"]);
            Assert.Equal(11, subject.GlobalScope.Top["x"]);
            Assert.Equal(27d, subject.GlobalScope.Top["c"]);
            Assert.Equal(25d, subject.GlobalScope.Top["b"]);
            Assert.Equal(2, subject.GlobalScope.Top["number"]);
        }

        [Fact]
        public void TestInterpretUnderscoreVarName()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                PROGRAM Part10;
                VAR
                    number, a, _b, c, x : integer;
                BEGIN
                    BEGIN
                        number := 2;
                        a := number;
                        _b := 10 * a + 10 * number div 4;
                        c := a - - _b
                    END;
                    x := 11;
                END.
                ");

            // Assert
            Assert.Equal(2, subject.GlobalScope.Top["a"]);
            Assert.Equal(11, subject.GlobalScope.Top["x"]);
            Assert.Equal(27d, subject.GlobalScope.Top["c"]);
            Assert.Equal(25d, subject.GlobalScope.Top["_b"]);
            Assert.Equal(2, subject.GlobalScope.Top["number"]);
        }

        [Fact]
        public void TestInterpretNumericVarName()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                PROGRAM Part10;
                VAR
                    number, a12, b, c, x : integer;
                BEGIN
                    BEGIN
                        number := 2;
                        a12 := number;
                        b := 10 * a12 + 10 * number div 4;
                        c := a12 - - b
                    END;
                    x := 11;
                END.
                ");

            // Assert
            Assert.Equal(2, subject.GlobalScope.Top["a12"]);
            Assert.Equal(11, subject.GlobalScope.Top["x"]);
            Assert.Equal(27d, subject.GlobalScope.Top["c"]);
            Assert.Equal(25d, subject.GlobalScope.Top["b"]);
            Assert.Equal(2, subject.GlobalScope.Top["number"]);
        }

        [Fact]
        public void TestInterpretV10()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                PROGRAM Part10;
                VAR
                   number     : INTEGER;
                   a, b, c, x : INTEGER;
                   y          : REAL;

                BEGIN {Part10}
                   BEGIN
                      number := 2;
                      a := number;
                      b := 10 * a + 10 * number DIV 4;
                      c := a - - b
                   END;
                   x := 11;
                   y := 20 / 7 + 3.14;
                   { writeln('a = ', a); }
                   { writeln('b = ', b); }
                   { writeln('c = ', c); }
                   { writeln('number = ', number); }
                   { writeln('x = ', x); }
                   { writeln('y = ', y); }
                END.  {Part10}
                ");

            // Assert
            Assert.Equal(2, subject.GlobalScope.Top["a"]);
            Assert.Equal(25d, subject.GlobalScope.Top["b"]);
            Assert.Equal(27d, subject.GlobalScope.Top["c"]);
            Assert.Equal(11, subject.GlobalScope.Top["x"]);
            Assert.Equal(2, subject.GlobalScope.Top["number"]);
            Assert.Equal(5.99714285714, Math.Round((double) subject.GlobalScope.Top["y"], 11));
        }

        [Fact]
        public void TestInterpretSymbolNotDeclaredException()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            var result = Assert.Throws<SymbolNotDeclaredException>(() =>
            {
                subject.Interpret(@"
                PROGRAM Part10;
                BEGIN
                    BEGIN
                        number := 2;
                        a12 := number;
                        b := 10 * a12 + 10 * number div 4;
                        c := a12 - - b
                    END;
                    x := 11;
                END.");
            });

            // Assert
            Assert.Equal("number", result.SymbolName);
        }

        [Fact]
        public void TestInterpretProcedure()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                 PROGRAM Part12;
                 VAR
                    a : INTEGER;
                 
                 PROCEDURE P1;
                 VAR
                    pa : REAL;
                    k : INTEGER;
                 
                    PROCEDURE P2;
                    VAR
                       ppa, z : INTEGER;
                    BEGIN {P2}
                       z := 777;
                    END;  {P2}
                 
                 BEGIN {P1}
                 
                 END;  {P1}
                 
                 BEGIN {Part12}
                    a := 10;
                 END.  {Part12}");

            // Assert
        }

        [Fact]
        public void TestInterpretProcedureCall()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                 program Main;
                    var x : integer;
                    procedure Alpha(a : integer; b : integer);
                       
                    begin
                       x := (a + b ) * 2;
                    end;
                 
                 begin { Main }
                 
                    Alpha(3 + 5, 7);  { procedure call }
                 
                 end.  { Main }");

            // Assert
            Assert.Equal(30d, subject.GlobalScope.Top["x"]);
        }

        [Fact]
        public void TestInterpretNestedProcedureCall()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                var y,z:integer;

                procedure Alpha(a : integer; b : integer);
                var x : integer;
                
                   procedure Beta(a : integer; b : integer);
                   var x : integer;
                   begin
                      x := a * 10 + b * 2;
                      z := x;
                   end;
                
                begin
                   x := (a + b ) * 2;
                
                   Beta(5, 10);      { procedure call }

                   y := x;
                end;
                
                begin { Main }
                
                   Alpha(3 + 5, 7);  { procedure call }
                
                end.  { Main }");

            // Assert
            Assert.Equal(30d, subject.GlobalScope.Top["y"]);
            Assert.Equal(70d, subject.GlobalScope.Top["z"]);
        }

        [Fact]
        public void TestInterpretIfThen()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                   var str : string;
                begin { Main }
                    if ( 1 = 1 ) then
                        str := 'Hello World!';
                end.  { Main }
                ");

            // Assert
            Assert.Equal("Hello World!", subject.GlobalScope.Top["str"]);
        }

        [Fact]
        public void TestInterpretIfThenElseElse()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                   var str : string;
                begin { Main }
                    if ( 1 = 0 ) then
                        str := 'Hello World!'
                    else
                        str := 'Bye World!';
                end.  { Main }
                ");

            // Assert
            Assert.Equal("Bye World!", subject.GlobalScope.Top["str"]);
        }

        [Fact]
        public void TestInterpretIfThenElseThen()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                   var str : string;
                begin { Main }
                    if ( 1 = 1 ) then
                        str := 'Hello World!'
                    else
                        str := 'Bye World!';
                end.  { Main }
                ");

            // Assert
            Assert.Equal("Hello World!", subject.GlobalScope.Top["str"]);
        }

        [Fact]
        public void TestInterpretStringIndex0()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                   var s : string;
                begin { Main }
                    s := 'Hello World!';
                    s := s[0];
                end.  { Main }
                ");

            // Assert
            Assert.Equal("H", subject.GlobalScope.Top["s"]);
        }

        [Fact]
        public void TestInterpretStringIndex2()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                   var s : string;
                begin { Main }
                    s := 'Hello World!';
                    s := s[2];
                end.  { Main }
                ");

            // Assert
            Assert.Equal("l", subject.GlobalScope.Top["s"]);
        }

        [Fact]
        public void TestInterpretLength()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
                program Main;
                   var s : string;
                        len : integer;
                begin { Main }
                    s := 'Hello World!';
                    len := Length(s);
                end.  { Main }
                ");

            // Assert
            Assert.Equal("Hello World!", subject.GlobalScope.Top["s"]);
            Assert.Equal(12, subject.GlobalScope.Top["len"]);
        }

        [Fact]
        public void TestInterpretWhileDo()
        {
            // Arrange
            var subject = new SimplePascalInterpreter();

            // Act
            subject.Interpret(@"
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
                       if (i - j=0) then
                          l := false;
                    end;
                end.  { Main }
                ");

            // Assert
            Assert.Equal(13, subject.GlobalScope.Top["j"]);
            Assert.Equal(13d, subject.GlobalScope.Top["i"]);
            Assert.Equal(false, subject.GlobalScope.Top["l"]);
        }
    }
}