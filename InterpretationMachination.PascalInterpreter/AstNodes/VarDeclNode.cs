using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class VarDeclNode<T> : AstNode<T> where T : Enum
    {
        public List<string> Variable { get; set; }
        public TypeNode<T> Type { get; set; }
    }
}