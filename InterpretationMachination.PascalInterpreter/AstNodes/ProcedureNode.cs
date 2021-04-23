using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class ProcedureNode<T> : AstNode<T> where T : Enum
    {
        public string Name { get; set; }
        public List<VarDeclNode<T>> Parameters { get; set; }
        public BlockNode<T> Block { get; set; }
    }
}