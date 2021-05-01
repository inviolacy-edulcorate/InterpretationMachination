using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;
using InterpretationMachination.DataStructures.SymbolTable.Symbols;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class TypeNode<T> : AstNode<T> where T : Enum
    {
        public string Type { get; set; }
    }

    public class AnonymousTypeNode<T> : TypeNode<T> where T : Enum
    {
        public Symbol Symbol { get; set; }
    }

    public class ArrayTypeNode<T> : TypeNode<T> where T : Enum
    {
        public TypeNode<T> Subscript { get; set; }
        public TypeNode<T> ArrayType { get; set; }
    }
}