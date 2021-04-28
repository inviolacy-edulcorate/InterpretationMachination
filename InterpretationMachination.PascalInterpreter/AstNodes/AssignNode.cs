using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    /// <summary>
    /// Represents an assignment operation.
    ///
    /// a := b;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssignNode<T> : AstNode<T> where T : Enum
    {
        /// <summary>
        /// The variable to assign.
        /// </summary>
        public VarNode<T> Variable { get; set; }

        /// <summary>
        /// The expression to get the value from.
        /// </summary>
        public AstNodeValue<T> Expr { get; set; }
    }
}