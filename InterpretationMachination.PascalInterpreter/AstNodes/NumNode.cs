using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class NumNode<T> : AstNode<T> where T : Enum
    {
        public object Value { get; set; }
    }
}