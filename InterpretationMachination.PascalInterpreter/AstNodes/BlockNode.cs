using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class BlockNode<T> : AstNode<T> where T : Enum
    {
        public List<AstNode<T>> Declarations { get; set; }
        public CompoundNode<T> CompoundStatement { get; set; }
    }
}