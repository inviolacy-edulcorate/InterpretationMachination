using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class FunctionCallNode<T> : AstNode<T> where T : Enum
    {
        public string FunctionName { get; set; }
        public List<AstNode<T>> Parameters { get; set; }
    }
}