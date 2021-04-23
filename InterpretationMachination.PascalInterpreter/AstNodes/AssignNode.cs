﻿using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;

namespace InterpretationMachination.PascalInterpreter.AstNodes
{
    public class AssignNode<T> : AstNode<T> where T : Enum
    {
        public VarNode<T> Variable { get; set; }
        public AstNode<T> Expr { get; set; }
    }
}