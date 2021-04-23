using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;
using InterpretationMachination.DataStructures.SymbolTable;

namespace InterpretationMachination.Interfaces.Interfaces
{
    public interface ISemanticAnalyzer<T> where T:Enum
    {
        void Analyze(AstNode<T> tree);

        ScopedSymbolTable CurrentScope { get; }
    }
}