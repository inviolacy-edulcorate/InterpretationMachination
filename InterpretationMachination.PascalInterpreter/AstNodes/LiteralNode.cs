using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    /// <summary>
    /// Represents a constant/literal of some type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LiteralNode<T> : AstNodeValue<T> where T : Enum
    {
        /// <summary>
        /// The actual value of this literal.
        /// </summary>
        public object Value { get; set; }
    }
}