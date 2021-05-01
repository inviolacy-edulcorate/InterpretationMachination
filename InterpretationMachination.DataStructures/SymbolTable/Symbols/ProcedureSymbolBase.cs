using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.SymbolTable.Symbols
{
    public class ProcedureSymbolBase : Symbol
    {
        public ProcedureSymbolBase(string name, List<VariableSymbol> parameters) : base(name)
        {
            Parameters = parameters;
        }

        public List<VariableSymbol> Parameters { get; }
    }
}