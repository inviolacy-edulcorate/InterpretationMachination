using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class ProcedureCallNode<T> : AstNode<T> where T : Enum
    {
        public string ProcedureName { get; set; }
        public List<AstNode<T>> Parameters { get; set; }
    }
}