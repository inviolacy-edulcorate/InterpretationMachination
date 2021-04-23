using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.SymbolTable
{
    public class FunctionSymbolBase : Symbol
    {
        public FunctionSymbolBase(string name, List<VariableSymbol> parameters, Symbol typeSymbol) : base(name)
        {
            Parameters = parameters;
            TypeSymbol = typeSymbol;
        }

        public List<VariableSymbol> Parameters { get; }


        /// <summary>
        /// Return type of the function.
        /// </summary>
        public Symbol TypeSymbol { get; }
    }
}