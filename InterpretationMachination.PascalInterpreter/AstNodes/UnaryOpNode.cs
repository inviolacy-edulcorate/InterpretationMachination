using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class UnaryOpNode<T> : AstNode<T> where T : Enum
    {
        public AstNode<T> Factor { get; set; }
    }
}