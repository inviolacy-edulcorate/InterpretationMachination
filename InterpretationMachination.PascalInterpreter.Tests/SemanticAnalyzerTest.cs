using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.DataStructures.SymbolTable;
using InterpretationMachination.DataStructures.Tokens;
using InterpretationMachination.Interfaces.Exceptions;
using InterpretationMachination.PascalInterpreter.AstNodes;
using Xunit;

namespace InterpretationMachination.PascalInterpreter.Tests
{
    [ExcludeFromCodeCoverage]
    public class SemanticAnalyzerTest
    {
        [Fact]
        public void TestTableBuilder()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(@"
                PROGRAM Part11;
                VAR
                   x : INTEGER;
                   y : REAL;
                
                BEGIN
                
                END.");

            // Act
            subject.VisitTree(ast);
            var result = ((ProcedureSymbol<PascalTokenType>) subject.CurrentScope.LookupSymbol("GLOBAL")).SymbolTable;

            // Assert
            Assert.Equal("x", result.LookupSymbol("x").Name);
            Assert.Equal("INTEGER", result.LookupSymbol("x").Type.Name);

            Assert.Equal("y", result.LookupSymbol("y").Name);
            Assert.Equal("REAL", result.LookupSymbol("y").Type.Name);
        }

        [Fact]
        public void TestVisitTreeSymbolNotDeclaredException()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(@"
                PROGRAM NameError1;
                VAR
                   a : INTEGER;
                
                BEGIN
                   a := 2 + b;
                END.");

            // Act
            var result = Assert.Throws<SymbolNotDeclaredException>(() => { subject.VisitTree(ast); });

            // Assert
            Assert.Equal("b", result.SymbolName);
        }

        [Fact]
        public void TestVisitTreeSymbolAlreadyDeclaredException()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(@"
                PROGRAM NameError1;
                VAR
                   a, a : INTEGER;
                
                BEGIN
                   
                END.");

            // Act
            var result = Assert.Throws<SymbolAlreadyDeclaredException>(() => { subject.VisitTree(ast); });

            // Assert
            Assert.Equal("a", result.SymbolName);
        }

        [Fact]
        public void TestVisitTreeTypeNodeIsNullException()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = new VarDeclNode<PascalTokenType>
            {
                Type = null,
                Variable = new List<string> {"Test"},
                Token = new GenericToken<PascalTokenType>(null, PascalTokenType.KwVar, null, 0, 0)
            };

            // Act
            var result = Assert.Throws<VariableDeclarationTypeMissingException>(() => { subject.VisitTree(ast); });

            // Assert
            Assert.Equal("Test", result.Variable);
        }

        [Fact]
        public void TestVisitTreeUnaryOp()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(@"
                PROGRAM NameError1;
                VAR
                   a, b : INTEGER;
                
                BEGIN
                   a := - b;
                END.");

            // Act
            subject.VisitTree(ast);
            var result = ((ProcedureSymbol<PascalTokenType>) subject.CurrentScope.LookupSymbol("GLOBAL")).SymbolTable;

            // Assert
            Assert.Equal("a", result.LookupSymbol("a").Name);
            Assert.Equal("INTEGER", result.LookupSymbol("a").Type.Name);

            Assert.Equal("b", result.LookupSymbol("b").Name);
            Assert.Equal("INTEGER", result.LookupSymbol("b").Type.Name);
        }

        [Fact]
        public void TestSymbolToString()
        {
            // Arrange
            var subject = new Symbol("name", new BuiltinSymbol("INTEGER"));

            // Act
            var result = subject.ToString();

            // Assert
            Assert.Equal("<name:INTEGER>", result);
        }

        [Fact]
        public void TestBuiltinSymbolToString()
        {
            // Arrange
            BuiltinSymbol subject = new BuiltinSymbol("INTEGER");

            // Act
            var result = subject.ToString();

            // Assert
            Assert.Equal("INTEGER", result);
        }

        // TODO: Not implemented as of now, for future reference.
        [Fact]
        public void TestBuiltinsScope()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                begin
                end.
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            Assert.Equal("BUILTINS", result.Name); // Test builtins is defined.
            Assert.Equal("Main", result.LookupSymbol("Main").Name); // Test Main is defined.
            Assert.Equal("INTEGER", result.LookupSymbol("INTEGER").Name); // Test Main is defined.
            Assert.Equal("REAL", result.LookupSymbol("REAL").Name); // Test Main is defined.
        }

        [Fact]
        public void TestGlobalScope()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                    var x,y,z : integer;
                begin
                end.
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            Assert.Equal("GLOBAL", globalSymbol.Name); // Test GLOBAL is defined.
            var globalScope =
                Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol).SymbolTable; // Test GLOBAL is defined.
            Assert.Equal("x", globalScope.LookupSymbol("x").Name);
            Assert.Equal("y", globalScope.LookupSymbol("y").Name);
            Assert.Equal("z", globalScope.LookupSymbol("z").Name);
            Assert.Equal("INTEGER", globalScope.LookupSymbol("z").Type.Name);
        }

        [Fact]
        public void TestGlobalProcedureScope()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var x, y: real;
                
                   procedure Alpha(a : integer);
                      var y : integer;
                   begin
                
                   end;
                
                begin { Main }
                
                end.  { Main }
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            Assert.Equal("GLOBAL", globalSymbol.Name); // Test GLOBAL is defined.
            var globalScope =
                Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol).SymbolTable; // Test GLOBAL is defined.
            Assert.Equal("x", globalScope.LookupSymbol("x").Name);
            Assert.Equal("REAL", globalScope.LookupSymbol("x").Type.Name);
            Assert.Equal("y", globalScope.LookupSymbol("y").Name);
            Assert.Equal("REAL", globalScope.LookupSymbol("y").Type.Name);

            var alphaScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("Alpha"))
                .SymbolTable;
            Assert.Equal("a", alphaScope.LookupSymbol("a").Name);
            Assert.Equal("INTEGER", alphaScope.LookupSymbol("a").Type.Name);
            Assert.Equal("y", alphaScope.LookupSymbol("y").Name);
            Assert.Equal("INTEGER", alphaScope.LookupSymbol("y").Type.Name);
        }

        [Fact]
        public void TestGlobalMultipleProcedureScope()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
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
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            Assert.Equal("GLOBAL", globalSymbol.Name); // Test GLOBAL is defined.
            var globalScope =
                Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol).SymbolTable; // Test GLOBAL is defined.
            Assert.Equal("x", globalScope.LookupSymbol("x").Name);
            Assert.Equal("REAL", globalScope.LookupSymbol("x").Type.Name);
            Assert.Equal("y", globalScope.LookupSymbol("y").Name);
            Assert.Equal("REAL", globalScope.LookupSymbol("y").Type.Name);

            var alphaAScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("AlphaA"))
                .SymbolTable;
            Assert.Equal("a", alphaAScope.LookupSymbol("a").Name);
            Assert.Equal("INTEGER", alphaAScope.LookupSymbol("a").Type.Name);
            Assert.Equal("y", alphaAScope.LookupSymbol("y").Name);
            Assert.Equal("INTEGER", alphaAScope.LookupSymbol("y").Type.Name);

            var alphaBScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("AlphaB"))
                .SymbolTable;
            Assert.Equal("a", alphaBScope.LookupSymbol("a").Name);
            Assert.Equal("INTEGER", alphaBScope.LookupSymbol("a").Type.Name);
            Assert.Equal("b", alphaBScope.LookupSymbol("b").Name);
            Assert.Equal("INTEGER", alphaBScope.LookupSymbol("b").Type.Name);
        }

        [Fact]
        public void TestGlobalNestedProcedureScope()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
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
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            Assert.Equal("GLOBAL", globalSymbol.Name); // Test GLOBAL is defined.
            var globalScope =
                Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol).SymbolTable; // Test GLOBAL is defined.
            AssertScopeSymbol("x", "REAL", globalScope);
            AssertScopeSymbol("y", "REAL", globalScope);
            AssertScopeSymbol("b", "REAL", globalScope);
            AssertScopeSymbol("z", "INTEGER", globalScope);

            var alphaAScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("AlphaA"))
                .SymbolTable;
            AssertScopeSymbol("a", "INTEGER", alphaAScope);
            AssertScopeSymbol("b", "INTEGER", alphaAScope);

            var betaScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(alphaAScope.LookupSymbol("Beta"))
                .SymbolTable;
            AssertScopeSymbol("c", "INTEGER", betaScope);
            AssertScopeSymbol("y", "INTEGER", betaScope);

            var gammaScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(betaScope.LookupSymbol("Gamma"))
                .SymbolTable;
            AssertScopeSymbol("c", "INTEGER", gammaScope);
            AssertScopeSymbol("x", "INTEGER", gammaScope);

            var alphaBScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("AlphaB"))
                .SymbolTable;
            AssertScopeSymbol("a", "INTEGER", alphaBScope);
            AssertScopeSymbol("c", "REAL", alphaBScope);
        }

        [Fact]
        public void TestGlobalNestedProcedureScopeWithCall()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
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
                    AlphaA(13);
                end.  { Main }
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            Assert.Equal("GLOBAL", globalSymbol.Name); // Test GLOBAL is defined.
            var globalScope =
                Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol).SymbolTable; // Test GLOBAL is defined.
            AssertScopeSymbol("x", "REAL", globalScope);
            AssertScopeSymbol("y", "REAL", globalScope);
            AssertScopeSymbol("b", "REAL", globalScope);
            AssertScopeSymbol("z", "INTEGER", globalScope);

            var alphaAScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("AlphaA"))
                .SymbolTable;
            AssertScopeSymbol("a", "INTEGER", alphaAScope);
            AssertScopeSymbol("b", "INTEGER", alphaAScope);

            var betaScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(alphaAScope.LookupSymbol("Beta"))
                .SymbolTable;
            AssertScopeSymbol("c", "INTEGER", betaScope);
            AssertScopeSymbol("y", "INTEGER", betaScope);

            var gammaScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(betaScope.LookupSymbol("Gamma"))
                .SymbolTable;
            AssertScopeSymbol("c", "INTEGER", gammaScope);
            AssertScopeSymbol("x", "INTEGER", gammaScope);

            var alphaBScope = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalScope.LookupSymbol("AlphaB"))
                .SymbolTable;
            AssertScopeSymbol("a", "INTEGER", alphaBScope);
            AssertScopeSymbol("c", "REAL", alphaBScope);
        }

        [Fact]
        public void TestAnalyzeProcedureCallIncorrectParamCount0()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
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
                    AlphaA();
                end.  { Main }
                "
            );

            // Act
            Assert.Throws<ProcedureCallWithIncorrectParameterCountException>(
                () => subject.Analyze(ast));

            // Assert
        }

        [Fact]
        public void TestAnalyzeProcedureCallIncorrectParamCountFew()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer; d : real);
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
                    AlphaA(32);
                end.  { Main }
                "
            );

            // Act
            Assert.Throws<ProcedureCallWithIncorrectParameterCountException>(
                () => subject.Analyze(ast));

            // Assert
        }

        [Fact]
        public void TestAnalyzeProcedureCallIncorrectParamCountMany()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
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
                    AlphaA(32, 45);
                end.  { Main }
                "
            );

            // Act
            Assert.Throws<ProcedureCallWithIncorrectParameterCountException>(
                () => subject.Analyze(ast));

            // Assert
        }

        [Fact]
        public void TestAnalyzeProcedureParameterCountSymbol0()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                    
                   procedure AlphaA;
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                begin { Main }
                end.  { Main }
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;

            var alphaASymbol = globalScope.LookupSymbol("AlphaA");
            var alphaA = Assert.IsType<ProcedureSymbol<PascalTokenType>>(alphaASymbol);
            Assert.Equal(0, alphaA.Parameters?.Count ?? 0);
        }

        [Fact]
        public void TestAnalyzeProcedureParameterCountSymbol1()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a : integer);
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                begin { Main }
                end.  { Main }
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;

            var alphaASymbol = globalScope.LookupSymbol("AlphaA");
            var alphaA = Assert.IsType<ProcedureSymbol<PascalTokenType>>(alphaASymbol);
            Assert.Equal(1, alphaA.Parameters?.Count ?? 0);
        }

        [Fact]
        public void TestAnalyzeProcedureParameterCountSymbol3()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var b, x, y : real;
                   var z : integer;
                
                   procedure AlphaA(a, b : integer; c : real);
                   begin { AlphaA }
                
                   end;  { AlphaA }
                
                begin { Main }
                end.  { Main }
                "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;

            var alphaASymbol = globalScope.LookupSymbol("AlphaA");
            var alphaA = Assert.IsType<ProcedureSymbol<PascalTokenType>>(alphaASymbol);
            Assert.Equal(3, alphaA.Parameters?.Count ?? 0);
        }

        [Fact]
        public void TestAnalyzeIfThen()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                 program Main;
                   var str : string;
                 begin { Main }
                     if ( 1 = 1 ) then
                         str := 'Hello World!';
                 end.  { Main }
               "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;
            AssertScopeSymbol("str", "STRING", globalScope);
        }

        [Fact]
        public void TestAnalyzeIfThenElse()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var str : string;
                begin { Main }
                    if ( 1 = 1 ) then
                        str := 'Hello World!'
                    else
                        str := 'Bye World!';
                end.
               "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;
            AssertScopeSymbol("str", "STRING", globalScope);
        }

        [Fact]
        public void TestAnalyzeStringIndex()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var s : string;
                begin { Main }
                    s := 'Hello World!';
                    s := s[0];
                end.  { Main }
               "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;
            AssertScopeSymbol("s", "STRING", globalScope);
        }

        [Fact]
        public void TestAnalyzeLength()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var s : string;
                       len : integer;
                begin { Main }
                    s := 'Hello World!';
                    len := Length(s);
                end.  { Main }
               "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;
            AssertScopeSymbol("s", "STRING", globalScope);
            AssertScopeSymbol("len", "INTEGER", globalScope);
        }

        [Fact]
        public void TestAnalyzeReadFile()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
                program Main;
                   var s : string;
                begin { Main }
                    s := ReadFile('./hello.txt');
                end.  { Main }
               "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
            var globalScope = global.SymbolTable;
            AssertScopeSymbol("s", "STRING", globalScope);
        }

        [Fact]
        public void TestAnalyzeWhileDo()
        {
            // Arrange
            var subject = new SemanticAnalyzer();
            var parser = new SimplePascalParser();
            var ast = parser.Parse(
                @"
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
               "
            );

            // Act
            subject.Analyze(ast);
            var result = subject.CurrentScope;

            // Assert
            var globalSymbol = result.LookupSymbol("GLOBAL");
            var global = Assert.IsType<ProcedureSymbol<PascalTokenType>>(globalSymbol);
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private void AssertScopeSymbol(string name, string type, ScopedSymbolTable scope)
        {
            Assert.Equal(name, scope.LookupSymbol(name).Name);
            Assert.Equal(type, scope.LookupSymbol(name).Type.Name);
        }
    }
}