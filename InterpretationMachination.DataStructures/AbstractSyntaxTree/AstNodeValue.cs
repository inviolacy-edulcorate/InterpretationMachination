using System;

namespace InterpretationMachination.DataStructures.AbstractSyntaxTree
{
    /// <summary>
    /// Represents an AST node which has a type/value. This can either be
    /// directly (through the token) or by interpreting it.
    ///
    /// In the case of the Pascal grammar, this can kind of represent an "expr" (expression) node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AstNodeValue<T> : AstNode<T> where T : Enum 
    {
        /// <summary>
        /// The type that will roll out of this node.
        /// </summary>
        public string Type { get; set; }
    }
}