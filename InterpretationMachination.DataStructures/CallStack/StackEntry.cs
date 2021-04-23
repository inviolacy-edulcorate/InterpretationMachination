using InterpretationMachination.DataStructures.SymbolTable;

namespace InterpretationMachination.DataStructures.CallStack
{
    public class StackEntry
    {
        public StackEntry(Symbol symbol, object value)
        {
            Symbol = symbol;
            Value = value;
        }

        public Symbol Symbol { get; }
        public object Value { get; set; }
    }
}