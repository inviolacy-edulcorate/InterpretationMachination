namespace InterpretationMachination.DataStructures.SymbolTable.Symbols
{
    public abstract class DataTypeSymbol : Symbol
    {
        protected DataTypeSymbol(string name) : base(name)
        {
        }
    }

    public abstract class ScalarDataTypeSymbol : DataTypeSymbol
    {
        protected ScalarDataTypeSymbol(string name) : base(name)
        {
        }
    }

    public class StandardScalarDataTypeSymbol : ScalarDataTypeSymbol
    {
        protected StandardScalarDataTypeSymbol(string name) : base(name)
        {
        }
    }

    public class UserDefinedScalarDataTypeSymbol : ScalarDataTypeSymbol
    {
        protected UserDefinedScalarDataTypeSymbol(string name) : base(name)
        {
        }
    }

    public class EnumeratedScalarDataTypeSymbol : ScalarDataTypeSymbol
    {
        protected EnumeratedScalarDataTypeSymbol(string name) : base(name)
        {
        }
    }

    public class SubrangeScalarDataTypeSymbol : ScalarDataTypeSymbol
    {
        public SubrangeScalarDataTypeSymbol(int rangeStart, int rangeEnd)
            : base($"{rangeStart}..{rangeEnd}")
        {
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
        }

        /// <summary>
        /// Inclusive start of the range.
        /// </summary>
        public int RangeStart { get; set; }

        /// <summary>
        /// Inclusive end of the range.
        /// </summary>
        public int RangeEnd { get; set; }
    }

    public class ArrayDataTypeSymbol : DataTypeSymbol
    {
        public ArrayDataTypeSymbol(Symbol subscriptType, Symbol arrayType)
            : base($"ARRAY[{subscriptType.Name}]OF{arrayType.Name}")
        {
            SubscriptType = subscriptType;
            ArrayType = arrayType;
        }

        public Symbol SubscriptType { get; set; }
        public Symbol ArrayType { get; set; }
    }
}