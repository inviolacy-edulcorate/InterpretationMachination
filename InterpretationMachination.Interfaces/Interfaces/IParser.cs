using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.Interfaces.Interfaces
{

    public interface IParser<T> where T : Enum
    {
        AstNode<T> Parse(string inputString);
    }
}