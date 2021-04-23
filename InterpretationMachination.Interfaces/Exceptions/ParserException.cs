using System;

namespace InterpretationMachination.Interfaces.Exceptions
{
    /// <summary>
    /// Exception for when an <see cref="IParser"/> implementation has an exception.
    /// </summary>
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }

    public class UnexpectedTokenTypeException : ParserException
    {
        private const string MessageFormat =
            "Can't read the expected type '{0}', found '{1}'. ({2}:{3})";

        public UnexpectedTokenTypeException(Enum expectedTokenType, Enum foundTokenType, int line, int col)
            : base(string.Format(MessageFormat, expectedTokenType, foundTokenType, line, col))
        {
            ExpectedTokenType = expectedTokenType;
            FoundTokenType = foundTokenType;
        }

        public Enum ExpectedTokenType { get; }
        public Enum FoundTokenType { get; }
    }
}