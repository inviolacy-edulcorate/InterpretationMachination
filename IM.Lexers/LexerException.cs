using System;

namespace IM.Lexers
{
    /// <summary>
    /// Exception for when an ILexer implementation has an exception.
    /// </summary>
    public class LexerException : Exception
    {
        public LexerException(string message) : base(message)
        {
        }
    }

    public class UnknownCharacterException : LexerException
    {
        private const string MessageFormat =
            "Character '{0}' is unknown to the lexer and can't be tokenized. ({1}:{2})";

        //$"Character '{character}' is unknown to the lexer and can't be tokenized. ({lineNumber}:{columnNumber})"
        public UnknownCharacterException(string character, int lineNumber, int columnNumber)
            : base(string.Format(MessageFormat, character, lineNumber, columnNumber))
        {
            Character = character;

            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public string Character { get; }

        public int LineNumber { get; }
        public int ColumnNumber { get; }
    }
}