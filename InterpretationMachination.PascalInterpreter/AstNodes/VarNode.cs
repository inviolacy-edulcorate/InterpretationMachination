using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    /// <summary>
    /// Represents a variable, either in assignment or in usage.
    ///
    /// a := b;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VarNode<T> : AstNodeValue<T> where T : Enum
    {
        /// <summary>
        /// The name/identifier of this variable.
        /// </summary>
        public string Name { get; set; }
    }
}