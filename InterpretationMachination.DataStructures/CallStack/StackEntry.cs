using InterpretationMachination.DataStructures.SymbolTable;
using InterpretationMachination.DataStructures.SymbolTable.Symbols;

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