using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class NoOpNode<T> : AstNode<T> where T : Enum
    {
    }
}