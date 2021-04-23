using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class FunctionDeclarationNode<T> : AstNode<T> where T : Enum
    {
        public string Name { get; set; }
        public List<VarDeclNode<T>> Parameters { get; set; }

        public AstNode<T> Block { get; set; }

        /// <summary>
        /// Return type of the function.
        /// </summary>
        public TypeNode<T> Type { get; set; }
    }
}