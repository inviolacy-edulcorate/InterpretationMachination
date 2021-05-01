using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.SymbolTable.Symbols
{
    public class BuiltinProcedureSymbol : ProcedureSymbolBase
    {
        public BuiltinProcedureSymbol(string name, List<VariableSymbol> parameters) : base(name, parameters)
        {
        }
    }
}