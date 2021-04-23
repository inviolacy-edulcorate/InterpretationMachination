using System;

namespace InterpretationMachination.DataStructures.Tokens
{
    public class GenericToken<T> where T : Enum
    {
        public GenericToken(TokenSet<T> tokenSet, T type, object value, int lineNumber, int columnNumber)
        {
            Type = type;
            TokenSet = tokenSet;
            Value = value;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public T Type { get; }

        public object Value { get; }

        public int LineNumber { get; }
        public int ColumnNumber { get; }

        private TokenSet<T> TokenSet { get; }

        public int ValueAsInt =>
            TokenSet.IsIntegerType(Type) ? (int) Value : throw new InvalidOperationException();

        public double ValueAsReal =>
            TokenSet.IsRealType(Type) ? (double) Value : throw new InvalidOperationException();

        public string ValueAsString =>
            TokenSet.IsStringType(Type) ? (string) Value : throw new InvalidOperationException();

        public bool ValueAsBoolean
        {
            get
            {
                if (TokenSet.IsBooleanType(Type))
                {
                    if (Value is string strval)
                    {
                        if (bool.TryParse(strval, out var result))
                        {
                            return result;
                        }
                    }

                    return (bool) Value;
                }
                else
                    throw new InvalidOperationException();
            }
        }
    }
}