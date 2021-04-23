using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.DataStructures.SymbolTable
{
    public class UserDefinedFunctionSymbol<T> : FunctionSymbolBase where T : Enum
    {
        public UserDefinedFunctionSymbol(string name, List<VariableSymbol> parameters, Symbol typeSymbol,
            AstNode<T> functionBody, ScopedSymbolTable symbolTable) : base(name, parameters, typeSymbol)
        {
            FunctionBody = functionBody;
            SymbolTable = symbolTable;
        }

        public ScopedSymbolTable SymbolTable { get; }

        public AstNode<T> FunctionBody { get; }
    }
}