using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class TypeNode<T> : AstNode<T> where T : Enum
    {
        public string Type { get; set; }
    }
}