using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.SymbolTable
{
    public class BuiltinFunctionSymbol : FunctionSymbolBase
    {
        public BuiltinFunctionSymbol(string name, List<VariableSymbol> parameters, Symbol typeSymbol) : base(name,
            parameters, typeSymbol)
        {
        }
    }
}