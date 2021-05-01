using System.Collections.Generic;
using InterpretationMachination.DataStructures.SymbolTable;

namespace InterpretationMachination.DataStructures.CallStack
{
    public class StackFrame
    {
        public StackFrame(ScopedSymbolTable symbolTable, string name, int level)
        {
            SymbolTable = symbolTable;
            Name = name;
            Level = level;
            Data = new Dictionary<string, StackEntry>();

            InitializeFromScopedSymbolTable();
        }

        public ScopedSymbolTable SymbolTable { get; }

        public Dictionary<string, StackEntry> Data { get; }

        public string Name { get; }
        public int Level { get; }

        public object this[string index]
        {
            get => Data[index.ToUpper()].Value;
            set => Data[index.ToUpper()].Value = value;
        }

        /// <summary>
        /// Initializes the Stack Frame based on the table that was passed to it.
        ///
        /// This allows for 'get' simplification (no null checks) and is possible
        /// due to the statically typed nature of the Pascal.
        /// </summary>
        private void InitializeFromScopedSymbolTable()
        {
            foreach (var symbol in SymbolTable.DeclaredSymbols)
            {
                Data[symbol.Name.ToUpper()] = new StackEntry(symbol, null);
            }
        }
    }
}