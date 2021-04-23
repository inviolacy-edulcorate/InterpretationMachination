using System;
using InterpretationMachination.DataStructures.Tokens;

namespace InterpretationMachination.DataStructures.AbstractSyntaxTree
{
    public abstract class AstNode<T> where T : Enum
    {
        public GenericToken<T> Token { get; set; }
    }
}