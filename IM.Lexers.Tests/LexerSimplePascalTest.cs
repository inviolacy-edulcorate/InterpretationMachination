using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InterpretationMachination.DataStructures.Tokens;
using Xunit;

namespace IM.Lexers.Tests
{
    [ExcludeFromCodeCoverage]
    public class LexerSimplePascalTest
    {
        private TokenSet<TestPascalTokenType> TokenSet
        {
            get
            {
                var ts = new TokenSet<TestPascalTokenType>
                {
                    ["."] = TestPascalTokenType.Dot,
                    [";"] = TestPascalTokenType.SemCol,
                    [":="] = TestPascalTokenType.OpAssign,
                    ["+"] = TestPascalTokenType.OpAdd,
                    ["-"] = TestPascalTokenType.OpSub,
                    ["*"] = TestPascalTokenType.OpMul,
                    ["("] = TestPascalTokenType.ParL,
                    [")"] = TestPascalTokenType.ParR,
                    ["/"] = TestPascalTokenType.OpDiv,
                    [":"] = TestPascalTokenType.Colon,
                    [","] = TestPascalTokenType.Comma,
                    ["="] = TestPascalTokenType.Equals,
                    ["["] = TestPascalTokenType.Brl,
                    ["]"] = TestPascalTokenType.Brr,
                };
                ts.IntegerTypes.Add(TestPascalTokenType.ConstInteger);
                ts.RealTypes.Add(TestPascalTokenType.ConstReal);
                ts.StringTypes.Add(TestPascalTokenType.Id);
                ts.StringTypes.Add(TestPascalTokenType.ConstString);
                ts.BooleanTypes.Add(TestPascalTokenType.KwFalse);
                ts.BooleanTypes.Add(TestPascalTokenType.KwTrue);

                ts.NumericRecognizePattern = @"[0-9]";
                ts.NumericPattern = @"[0-9.]";
                ts.IdRecognizePattern = @"[a-zA-Z_]";
                ts.IdPattern = @"[a-zA-Z0-9_]";

                ts.WhitespaceCharacters.Add("\n");
                ts.WhitespaceCharacters.Add("\r");
                ts.WhitespaceCharacters.Add("\t");
                ts.WhitespaceCharacters.Add(" ");

                ts.NewLineCharacters.Add("\n");

                ts.CommentStartEndCharacters["{"] = "}";

                ts.EndOfStreamTokenType = TestPascalTokenType.EndOfFile;
                ts.IdTokenType = TestPascalTokenType.Id;
                ts.DoubleTokenType = TestPascalTokenType.ConstReal;
                ts.IntegerTokenType = TestPascalTokenType.ConstInteger;
                ts.StringTokenType = TestPascalTokenType.ConstString;
                
                ts.StringStartCharacter = "'";

                return ts;
            }
        }

        private Dictionary<string, TestPascalTokenType> KeyWords => new Dictionary<string, TestPascalTokenType>
        {
            {"BEGIN", TestPascalTokenType.KwBegin},
            {"END", TestPascalTokenType.KwEnd},
            {"DIV", TestPascalTokenType.OpIntDiv},
            {"PROGRAM", TestPascalTokenType.KwProgram},
            {"REAL", TestPascalTokenType.TypeReal},
            {"INTEGER", TestPascalTokenType.TypeInteger},
            {"STRING", TestPascalTokenType.TypeString},
            {"VAR", TestPascalTokenType.KwVar},
            {"PROCEDURE", TestPascalTokenType.KwProcedure},
            {"FUNCTION", TestPascalTokenType.KwFunction},
            {"IF", TestPascalTokenType.KwIf},
            {"THEN", TestPascalTokenType.KwThen},
            {"ELSE", TestPascalTokenType.KwElse},
            {"TRUE", TestPascalTokenType.KwTrue},
            {"FALSE", TestPascalTokenType.KwFalse},
            {"BOOLEAN", TestPascalTokenType.TypeBoolean},
            {"WHILE", TestPascalTokenType.KwWhile},
            {"DO", TestPascalTokenType.KwDo},
        };

        [Fact]
        public void TestGetNextTokenDot()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@".", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.Dot, result.Type);
        }

        [Fact]
        public void TestGetNextTokenBegin()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"BEGIN", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.KwBegin, result.Type);
        }

        [Fact]
        public void TestGetNextTokenEnd()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"END", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.KwEnd, result.Type);
        }

        [Fact]
        public void TestGetNextTokenSemCol()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@";", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.SemCol, result.Type);
        }

        [Fact]
        public void TestGetNextTokenId()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"someVarName", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.Id, result.Type);
            Assert.Equal("someVarName", result.Value);
        }

        [Fact]
        public void TestGetNextTokenIdWithNumeric()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"someVarName123", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.Id, result.Type);
            Assert.Equal("someVarName123", result.Value);
        }

        [Fact]
        public void TestGetNextTokenIdWithUnderscore()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"_someVarName", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.Id, result.Type);
            Assert.Equal("_someVarName", result.Value);
        }

        [Fact]
        public void TestGetNextTokenInteger()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"789845", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ConstInteger, result.Type);
            Assert.Equal(789845, result.ValueAsInt);
        }

        [Fact]
        public void TestGetNextTokenAdd()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"+", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.OpAdd, result.Type);
        }

        [Fact]
        public void TestGetNextTokenSub()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"-", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.OpSub, result.Type);
        }

        [Fact]
        public void TestGetNextTokenMul()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"*", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.OpMul, result.Type);
        }

        [Fact]
        public void TestGetNextTokenDiv()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"/", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.OpDiv, result.Type);
        }


        [Fact]
        public void TestGetNextTokenParL()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@"(", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ParL, result.Type);
        }

        [Fact]
        public void TestGetNextTokenParR()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@")", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ParR, result.Type);
        }

        [Fact]
        public void TestGetNextTokenWhitespace()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@" ", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.EndOfFile, result.Type);
        }

        [Fact]
        public void TestGetNextTokenNewLine()
        {
            AssertTokenTypeFromString(TestPascalTokenType.EndOfFile, "\n");
        }

        [Fact]
        public void TestGetNextTokenWhitespaceThenToken()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(@" a", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.Id, result.Type);
        }

        [Fact]
        public void TestGetNextTokenNewLineThenToken()
        {
            AssertTokenTypeFromString(TestPascalTokenType.Id, "\na");
        }

        [Fact]
        public void TestGetNextTokenKwProgram()
        {
            AssertTokenTypeFromString(TestPascalTokenType.KwProgram, "PROGRAM");
        }

        [Fact]
        public void TestGetNextTokenConstReal()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("24.345", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ConstReal, result.Type);
            Assert.Equal(24.345, result.ValueAsReal);
        }

        [Fact]
        public void TestGetNextTokenOpIntDiv()
        {
            AssertTokenTypeFromString(TestPascalTokenType.OpIntDiv, "DIV");
        }

        [Fact]
        public void TestGetNextTokenKwVar()
        {
            AssertTokenTypeFromString(TestPascalTokenType.KwVar, "VAR");
        }

        [Fact]
        public void TestGetNextTokenTypeInteger()
        {
            AssertTokenTypeFromString(TestPascalTokenType.TypeInteger, "INTEGER");
        }

        [Fact]
        public void TestGetNextTokenTypeReal()
        {
            AssertTokenTypeFromString(TestPascalTokenType.TypeReal, "REAL");
        }

        [Fact]
        public void TestGetNextTokenComma()
        {
            AssertTokenTypeFromString(TestPascalTokenType.Comma, ",");
        }

        [Fact]
        public void TestGetNextTokenColon()
        {
            AssertTokenTypeFromString(TestPascalTokenType.Colon, ":");
        }

        [Fact]
        public void TestGetNextTokenCommentEmpty()
        {
            AssertTokenTypeFromString(TestPascalTokenType.EndOfFile, "{}");
        }

        [Fact]
        public void TestGetNextTokenCommentFilled()
        {
            AssertTokenTypeFromString(TestPascalTokenType.EndOfFile, "{Hello}");
        }

        [Fact]
        public void TestGetNextTokenCommentFilledTokenAfter()
        {
            AssertTokenTypeFromString(TestPascalTokenType.Id, "{Hello}a");
        }

        [Fact]
        public void TestGetNextTokenAssignment()
        {
            AssertTokenTypeFromString(TestPascalTokenType.OpAssign, ":=");
        }

        [Fact]
        public void TestGetNextTokenIntDivAfterId()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("number DIV 4", TokenSet, KeyWords);
            subject.GetNextToken();

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.OpIntDiv, result.Type);
        }

        [Fact]
        public void TestGetNextTokenKwProcedure()
        {
            AssertTokenTypeFromString(TestPascalTokenType.KwProcedure, "PROCEDURE");
        }

        [Fact]
        public void TestGetNextTokenLineNumber1()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("PROCEDURE", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(1, result.LineNumber);
        }

        [Fact]
        public void TestGetNextTokenLineNumber2()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>($"{Environment.NewLine}PROCEDURE", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(2, result.LineNumber);
        }

        [Fact]
        public void TestGetNextTokenLineNumber3()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>($"{Environment.NewLine}{Environment.NewLine}PROCEDURE", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(3, result.LineNumber);
        }

        [Fact]
        public void TestGetNextTokenLineNumber3WithOtherToken()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>($"{Environment.NewLine}PROCEDURE{Environment.NewLine}PROCEDURE", TokenSet, KeyWords);
            subject.GetNextToken();

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(3, result.LineNumber);
        }

        [Fact]
        public void TestGetNextTokenColumnNumber1()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("PROCEDURE", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(1, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenColumnNumber2WhiteSpace()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(" PROCEDURE", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(2, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenColumnNumber2Token()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(":PROCEDURE", TokenSet, KeyWords);
            subject.GetNextToken();

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(2, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenColumnNumber11Token()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("PROCEDURE :", TokenSet, KeyWords);
            subject.GetNextToken();

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(11, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenColumnNumber1NewLine()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>($"      {Environment.NewLine}PROCEDURE", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(1, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenUnknownTokenException1()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("$", TokenSet, KeyWords);

            // Act
            var result = Assert.Throws<UnknownCharacterException>(() => subject.GetNextToken());

            // Assert
            Assert.Equal("$", result.Character);
            Assert.Equal(1, result.LineNumber);
            Assert.Equal(1, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenUnknownTokenException2()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>("PROCEDURE$", TokenSet, KeyWords);
            subject.GetNextToken();

            // Act
            var result = Assert.Throws<UnknownCharacterException>(() => subject.GetNextToken());

            // Assert
            Assert.Equal("$", result.Character);
            Assert.Equal(1, result.LineNumber);
            Assert.Equal(10, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenUnknownTokenException3()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>($"PROCEDURE{Environment.NewLine}$", TokenSet, KeyWords);
            subject.GetNextToken();

            // Act
            var result = Assert.Throws<UnknownCharacterException>(() => subject.GetNextToken());

            // Assert
            Assert.Equal("$", result.Character);
            Assert.Equal(2, result.LineNumber);
            Assert.Equal(1, result.ColumnNumber);
        }

        [Fact]
        public void TestGetNextTokenConstString()
        {
            // Arrange
            var str = "ASDF / _ + -";
            var subject = new Lexer<TestPascalTokenType>($"'{str}'", TokenSet, KeyWords);


            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ConstString, result.Type);
            Assert.Equal(str, result.Value);
        }

        [Fact]
        public void TestGetNextTokenConstStringEscapedQuote()
        {
            // Arrange
            var str = "ASDF / ''_ + -";
            var subject = new Lexer<TestPascalTokenType>($"'{str}'", TokenSet, KeyWords);


            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ConstString, result.Type);
            Assert.Equal(str.Replace("''", "'"), result.Value);
        }

        [Fact]
        public void TestGetNextTokenConstStringEscapedQuoteEnd()
        {
            // Arrange
            var str = "ASDF / _ + -''";
            var subject = new Lexer<TestPascalTokenType>($"'{str}'", TokenSet, KeyWords);


            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.ConstString, result.Type);
            Assert.Equal(str.Replace("''", "'"), result.Value);
        }

        [Fact]
        public void TestGetNextTokenKwFunction()
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>($"FUNCTION", TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(TestPascalTokenType.KwFunction, result.Type);
        }

        [Fact]
        public void TestGetNextTokenTypeString()
        {
            AssertTokenTypeFromString(TestPascalTokenType.TypeString, "STRING");
        }

        [Fact]
        public void TestGetNextTokenKwIf()
        {
            AssertTokenTypeFromString(TestPascalTokenType.KwIf, "IF");
        }

        [Fact]
        public void TestGetNextTokenKwThen()
        {
            AssertTokenTypeFromString(TestPascalTokenType.KwThen, "THEN");
        }

        [Fact]
        public void TestGetNextTokenKwElse()
        {
            AssertTokenTypeFromString(TestPascalTokenType.KwElse, "ELSE");
        }

        [Fact]
        public void TestGetNextTokenEquals()
        {
            AssertTokenTypeFromString(TestPascalTokenType.Equals, "=");
        }

        [Fact]
        public void TestGetNextTokenTrue()
        {
            var r = AssertTokenTypeFromString(TestPascalTokenType.KwTrue, "TRUE");

            Assert.True(r.ValueAsBoolean);
        }

        [Fact]
        public void TestGetNextTokenFalse()
        {
            var r = AssertTokenTypeFromString(TestPascalTokenType.KwFalse, "FALSE");

            Assert.False(r.ValueAsBoolean);
        }

        [Fact]
        public void TestGetNextTokenBooleanType()
        {
            AssertTokenTypeFromString(TestPascalTokenType.TypeBoolean, "BOOLEAN");
        }

        [Fact]
        public void TestGetNextTokenBrl()
        {
            AssertTokenTypeFromString(TestPascalTokenType.Brl, "[");
        }

        [Fact]
        public void TestGetNextTokenBrr()
            => AssertTokenTypeFromString(TestPascalTokenType.Brr, "]");

        [Fact]
        public void TestGetNextTokenWhile()
            => AssertTokenTypeFromString(TestPascalTokenType.KwWhile, "WHILE");

        [Fact]
        public void TestGetNextTokenDo()
            => AssertTokenTypeFromString(TestPascalTokenType.KwDo, "DO");

        private GenericToken<TestPascalTokenType> AssertTokenTypeFromString(TestPascalTokenType tokenType, string str)
        {
            // Arrange
            var subject = new Lexer<TestPascalTokenType>(str, TokenSet, KeyWords);

            // Act
            var result = subject.GetNextToken();

            // Assert
            Assert.Equal(tokenType, result.Type);

            return result;
        }
    }
}