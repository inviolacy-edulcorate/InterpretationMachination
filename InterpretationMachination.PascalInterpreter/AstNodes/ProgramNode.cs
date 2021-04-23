using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class ProgramNode<T> : AstNode<T> where T : Enum
    {
        public VarNode<T> Name { get; set; }
        public BlockNode<T> Block { get; set; }
    }
}