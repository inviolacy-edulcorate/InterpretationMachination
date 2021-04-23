using System;

namespace InterpretationMachination.Interfaces.Exceptions
{
    /// <summary>
    /// Exception for when an Semantic Analysis implementation has an exception.
    /// </summary>
    public class SemanticAnalysisException : Exception
    {
        public SemanticAnalysisException(string message) : base(message)
        {
        }
    }

    public class VariableDeclarationTypeMissingException : SemanticAnalysisException
    {
        private const string MessageFormat = "Variable declaration for '{0}' is missing a type. ({1},{2})";

        public VariableDeclarationTypeMissingException(string variable, int line, int col)
            : base(string.Format(MessageFormat, variable, line, col))
        {
            Variable = variable;
        }

        public string Variable { get; }
    }

    public class SymbolNotDeclaredException : SemanticAnalysisException
    {
        public SymbolNotDeclaredException(string symbolName) : base(
            $"Symbol '{symbolName}' is used before it is declared.")
        {
            SymbolName = symbolName;
        }

        public string SymbolName { get; }
    }

    public class SymbolAlreadyDeclaredException : SemanticAnalysisException
    {
        public SymbolAlreadyDeclaredException(string symbolName) : base(
            $"Symbol '{symbolName}' is used before it is declared.")
        {
            SymbolName = symbolName;
        }

        public string SymbolName { get; }
    }

    public class TypeNotDeclaredException : SemanticAnalysisException
    {
        public TypeNotDeclaredException(string typeName) : base(
            $"Symbol '{typeName}' is used before it is declared.")
        {
            TypeName = typeName;
        }

        public string TypeName { get; }
    }

    public class ProcedureCallWithIncorrectParameterCountException : SemanticAnalysisException
    {
        private const string MessageFormat = "Procedure call '{0}' is called with the wrong number of parameters. ({1},{2})";

        public ProcedureCallWithIncorrectParameterCountException(string procedure, int line, int col)
            : base(string.Format(MessageFormat, procedure, line, col))
        {
            Procedure = procedure;
        }

        public string Procedure { get; }
    }

    public class ProcedureNotDefinedException : SemanticAnalysisException
    {
        private const string MessageFormat = "Procedure '{0}' is not defined. ({1},{2})";

        public ProcedureNotDefinedException(string procedure, int line, int col)
            : base(string.Format(MessageFormat, procedure, line, col))
        {
            Procedure = procedure;
        }

        public string Procedure { get; }
    }
}