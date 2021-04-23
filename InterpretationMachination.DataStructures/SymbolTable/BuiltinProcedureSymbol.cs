using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.SymbolTable
{
    public class BuiltinProcedureSymbol : ProcedureSymbolBase
    {
        public BuiltinProcedureSymbol(string name, List<VariableSymbol> parameters) : base(name, parameters)
        {
        }
    }
}