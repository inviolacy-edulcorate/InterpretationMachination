using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class VarNode<T> : AstNode<T> where T : Enum
    {
        public string Name { get; set; }
    }
}