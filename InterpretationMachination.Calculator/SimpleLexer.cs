using System;
using InterpretationMachination.DataStructures.Tokens;

namespace InterpretationMachination.Calculator
{
    public class SimpleLexer
    {
        public SimpleLexer()
        {
            Position = -1;

            TokenSet = new TokenSet<TokenType>
            {
                ["+"] = TokenType.OpAdd,
                ["-"] = TokenType.OpSub,
                ["*"] = TokenType.OpMul,
                ["("] = TokenType.ParL,
                [")"] = TokenType.ParR,
                ["/"] = TokenType.OpDiv
            };
            TokenSet.IntegerTypes.Add(TokenType.Integer);
        }

        public string Text { get; private set; }

        /// <summary>
        /// What character is the current (ie, the next) to be read.
        /// </summary>
        public int Position { get; private set; }

        public int NextPosition => Position + 1;

        public char? CurrentChar => (0 <= Position && Position < Text.Length) ? Text[Position] : (char?) null;

        public char? NextChar => NextPosition < Text.Length ? Text[NextPosition] : (char?) null;

        public TokenSet<TokenType> TokenSet { get; set; }

        public void InputString(string input)
        {
            Text = input;
            Position = 0;
        }

        /// <summary>
        /// Reads the next token from the text.
        /// </summary>
        /// <returns></returns>
        public GenericToken<TokenType> GetNextToken()
        {
            while (CurrentChar != null)
            {
                if (CurrentChar is ' ')
                {
                    HandleWhitespace();
                }
                else if (CharIsInt(CurrentChar))
                {
                    var value = HandleInteger();

                    return new GenericToken<TokenType>(TokenSet, TokenType.Integer, value, 0, 0);
                }
                else if (CharIsOperator(CurrentChar))
                {
                    char c = CurrentChar.Value;

                    var opType = HandleOperator();

                    return new GenericToken<TokenType>(TokenSet, opType, c, 0, 0);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"The current char is not known by the lexer! Char: '{CurrentChar}'");
                }
            }

            return new GenericToken<TokenType>(TokenSet, TokenType.EndOfFile, null, 0, 0);
        }

        /// <summary>
        /// Reads the current char as an operator type.
        /// </summary>
        /// <returns>The token type of the operator read.</returns>
        private TokenType HandleOperator()
        {
            var tokenType = CurrentChar switch
            {
                '/' => TokenType.OpDiv,
                '*' => TokenType.OpMul,
                '+' => TokenType.OpAdd,
                '-' => TokenType.OpSub,
                '(' => TokenType.ParL,
                ')' => TokenType.ParR,
                _ => throw new InvalidOperationException("The current char is not an operator! Char: '{CurrentChar}'")
            };

            // Handle functions are expected to move the Position to where the next line should read.
            Advance();

            return tokenType;
        }

        /// <summary>
        /// Loops until a non-whitespace character is found.
        /// </summary>
        private void HandleWhitespace()
        {
            while (CurrentChar == ' ')
            {
                Advance();
            }
        }

        /// <summary>
        /// Loops to read all characters of an integer.
        /// </summary>
        /// <returns>An integer which might consist of many characters.</returns>
        private int HandleInteger()
        {
            var chars = "";

            while (CharIsInt(CurrentChar))
            {
                chars += CurrentChar;

                Advance();
            }

            return int.Parse(chars);
        }

        /// <summary>
        /// Advance the lexer one character.
        /// </summary>
        private void Advance()
        {
            Position += 1;
        }

        /// <summary>
        /// Check if the char can parse to int.
        /// </summary>
        /// <param name="c">Char to attempt to parse.</param>
        /// <returns>True if int, false if not int.</returns>
        private bool CharIsInt(char? c)
        {
            return int.TryParse(CurrentChar.ToString(), out _);
        }

        private bool CharIsOperator(char? c)
        {
            return c switch
            {
                '/' => true,
                '*' => true,
                '+' => true,
                '-' => true,
                '(' => true,
                ')' => true,
                _ => false
            };
        }
    }
}