using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.DataStructures.SymbolTable
{
    /// <summary>
    /// A symbol which represents a procedure.
    /// </summary>
    public class ProcedureSymbol<T> : ProcedureSymbolBase where T : Enum
    {
        public ProcedureSymbol(
            string name,
            List<VariableSymbol> parameters,
            ScopedSymbolTable symbolTable,
            AstNode<T> procedureBody) : base(name, parameters)
        {
            Parameters = parameters;
            SymbolTable = symbolTable;
            ProcedureBody = procedureBody;
        }

        public List<VariableSymbol> Parameters { get; }

        public ScopedSymbolTable SymbolTable { get; }

        public AstNode<T> ProcedureBody { get; }
    }
}