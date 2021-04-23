using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class CompoundNode<T> : AstNode<T> where T : Enum
    {
        public List<AstNode<T>> Children { get; set; }
    }
}