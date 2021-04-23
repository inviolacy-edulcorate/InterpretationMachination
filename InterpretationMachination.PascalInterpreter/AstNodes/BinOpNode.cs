
using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class BinOpNode<T> : AstNode<T> where T : Enum
    {
        public AstNode<T> Left { get; set; }
        public AstNode<T> Right { get; set; }
    }
}