using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IM.Lexers.Interfaces;
using InterpretationMachination.DataStructures.Tokens;

namespace IM.Lexers
{
    /// <summary>
    /// Read Whitespace
    /// Read NewLines
    /// Read Strings
    /// Read KeyWords
    /// Read IDs
    ///
    /// Supports up to 2 chars per char.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Lexer<T> : ILexer<T> where T : Enum
    {
        public Lexer(string input, TokenSet<T> tokenSet, IDictionary<string, T> keyWords)
        {
            TokenSet = tokenSet;
            KeyWords = keyWords;
            Text = input;

            LineNumber = 1;
            ColumnNumber = 1;
        }

        public TokenSet<T> TokenSet { get; }

        public IDictionary<string, T> KeyWords { get; }

        private string Text { get; }

        private string CurrentChar => LookAhead(0);

        /// <summary>
        /// What character is the current (ie, the next) to be read.
        /// </summary>
        private int Position { get; set; }

        private int LineNumber { get; set; }

        private int ColumnNumber { get; set; }

        public GenericToken<T> GetNextToken()
        {
            while (CurrentChar != null)
            {
                // Do nothing with whitespace.
                if (TokenSet.WhitespaceCharacters.Contains(CurrentChar))
                {
                    Advance();
                    continue;
                }

                if (TokenSet.CommentStartEndCharacters.ContainsKey(CurrentChar))
                {
                    var endChar = TokenSet.CommentStartEndCharacters[CurrentChar];
                    Advance();

                    while (CurrentChar != endChar)
                    {
                        Advance();
                    }

                    Advance();
                    continue;
                }

                // Handle tokens registered in TokenSet.
                var tokenTypeFromTokenSet =
                    TokenSet[CurrentChar + LookAhead(1)].Equals(default(T))
                        ? TokenSet[CurrentChar]
                        : TokenSet[CurrentChar + LookAhead(1)];

                if (!tokenTypeFromTokenSet.Equals(default(T)))
                {
                    int line = LineNumber, col = ColumnNumber;

                    Advance(TokenSet[tokenTypeFromTokenSet].Length);

                    return new GenericToken<T>(
                        TokenSet,
                        tokenTypeFromTokenSet,
                        TokenSet[tokenTypeFromTokenSet], line, col);
                }

                if (Regex.IsMatch(CurrentChar, TokenSet.IdRecognizePattern))
                {
                    return GetId();
                }

                if (Regex.IsMatch(CurrentChar, TokenSet.NumericRecognizePattern))
                {
                    return GetInt();
                }

                if (CurrentChar == TokenSet.StringStartCharacter)
                {
                    return GetString();
                }

                throw new UnknownCharacterException(CurrentChar, LineNumber, ColumnNumber);
            }

            return new GenericToken<T>(TokenSet, TokenSet.EndOfStreamTokenType, null, LineNumber,
                ColumnNumber);
        }

        #region IEnumerable Implementation

        public IEnumerator<GenericToken<T>> GetEnumerator()
        {
            while (CurrentChar != null)
            {
                yield return GetNextToken();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private void Advance()
        {
            // If we find a new-line, move the line number up one.
            if (TokenSet.NewLineCharacters.Contains(CurrentChar))
            {
                LineNumber += 1;
                ColumnNumber = 0;
            }

            ColumnNumber += 1;

            Position += 1;
        }

        /// <summary>
        /// Advance the pointer by x.
        /// </summary>
        private void Advance(int x)
        {
            for (int i = 0; i < x; i++)
            {
                Advance();
            }
        }

        /// <summary>
        /// Look <paramref name="index"/> ahead of <see cref="CurrentChar"/>.
        /// </summary>
        /// <param name="index">Index to look-ahead by.</param>
        /// <returns>Character at look-ahead position, null if out of bounds.</returns>
        private string LookAhead(int index)
        {
            var readPos = Position + index;
            return 0 <= readPos && readPos < Text.Length
                ? Text[readPos].ToString()
                : null;
        }

        /// <summary>
        /// Read an integer.
        /// TODO And REAL.
        /// </summary>
        /// <returns>The read integer.</returns>
        private GenericToken<T> GetInt()
        {
            string tokenText = string.Empty;
            int line = LineNumber, col = ColumnNumber;

            // Is regex okay?
            while (CurrentChar != null &&
                   Regex.IsMatch(CurrentChar, TokenSet.NumericPattern))
            {
                tokenText += CurrentChar;

                Advance();
            }

            if (int.TryParse(tokenText, out int tokenInt))
            {
                return new GenericToken<T>(TokenSet, TokenSet.IntegerTokenType, tokenInt, line, col);
            }

            if (double.TryParse(tokenText, out double tokenDouble))
            {
                return new GenericToken<T>(TokenSet, TokenSet.DoubleTokenType, tokenDouble, line, col);
            }

            throw new InvalidOperationException($"Token text '{tokenText}' can't be parsed!");
        }

        private GenericToken<T> GetString()
        {
            string tokenText = string.Empty;
            int line = LineNumber, col = ColumnNumber;

            Advance();
            // TODO Handle escape characters if needed.
            while (CurrentChar != TokenSet.StringStartCharacter ||
                   (CurrentChar == TokenSet.StringStartCharacter && LookAhead(1) == TokenSet.StringStartCharacter) // '' Escape.
            )
            {
                tokenText += CurrentChar;
                if (CurrentChar == TokenSet.StringStartCharacter && LookAhead(1) == TokenSet.StringStartCharacter)
                {
                    Advance();
                }

                Advance();
            }

            // Advance to consume the final ' .
            Advance();

            return new GenericToken<T>(TokenSet, TokenSet.StringTokenType, tokenText, line, col);
        }


        /// <summary>
        /// Read a single ID from the assumption the first character has been checked.
        /// </summary>
        /// <returns>Token of the ID, either reserved or not.</returns>
        private GenericToken<T> GetId()
        {
            string tokenText = string.Empty;
            int line = LineNumber, col = ColumnNumber;

            // Is regex okay?
            while (CurrentChar != null &&
                   Regex.IsMatch(CurrentChar, TokenSet.IdPattern))
            {
                tokenText += CurrentChar;

                Advance();
            }

            // If this is a reserved keyword, return it.
            return
                KeyWords.ContainsKey(tokenText.ToUpper())
                    ? new GenericToken<T>(TokenSet, KeyWords[tokenText.ToUpper()], tokenText, line, col)
                    : new GenericToken<T>(TokenSet, TokenSet.IdTokenType, tokenText, line, col);
        }
    }
}