using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class IfThenNode<T> : AstNode<T> where T : Enum
    {
        public AstNode<T> Condition { get; set; }
        public AstNode<T> Then { get; set; }
        public AstNode<T> Else { get; set; }
    }
}