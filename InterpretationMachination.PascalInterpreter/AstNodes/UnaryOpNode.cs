using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class UnaryOpNode<T> : AstNodeValue<T> where T : Enum
    {
        public AstNodeValue<T> Factor { get; set; }
    }
}