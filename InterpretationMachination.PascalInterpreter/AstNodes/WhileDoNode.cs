using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class WhileDoNode<T> : AstNode<T> where T : Enum
    {
        public AstNode<T> Condition { get; set; }
        public AstNode<T> Statement { get; set; }
    }
}