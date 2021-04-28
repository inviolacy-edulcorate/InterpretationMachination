
using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class BinOpNode<T> : AstNodeValue<T> where T : Enum
    {
        public AstNodeValue<T> Left { get; set; }
        public AstNodeValue<T> Right { get; set; }
    }
}