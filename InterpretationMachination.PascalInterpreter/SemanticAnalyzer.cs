using System.Collections.Generic;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;
using InterpretationMachination.DataStructures.SymbolTable;
using InterpretationMachination.DataStructures.SymbolTable.Symbols;
using InterpretationMachination.Interfaces.Exceptions;
using InterpretationMachination.Interfaces.Interfaces;
using InterpretationMachination.PascalInterpreter.AstNodes;

namespace InterpretationMachination.PascalInterpreter
{
    /// <summary>
    /// Symbol table builder for Pascal.
    /// </summary>
    public class SemanticAnalyzer : AstVisitor<PascalTokenType>, ISemanticAnalyzer<PascalTokenType>
    {
        public SemanticAnalyzer()
        {
            CurrentScope = new ScopedSymbolTable();

            CurrentScope.DeclareSymbol(new BuiltinSymbol("INTEGER"));
            CurrentScope.DeclareSymbol(new BuiltinSymbol("REAL"));
            CurrentScope.DeclareSymbol(new BuiltinSymbol("STRING"));
            CurrentScope.DeclareSymbol(new BuiltinSymbol("BOOLEAN"));
            CurrentScope.DeclareSymbol(new BuiltinProcedureSymbol("WRITELN",
                new List<VariableSymbol>
                {
                    new VariableSymbol("toWrite", CurrentScope.LookupSymbol("INTEGER"))
                }));
            CurrentScope.DeclareSymbol(new BuiltinFunctionSymbol("READFILE",
                new List<VariableSymbol>
                {
                    new VariableSymbol("filePathToRead", CurrentScope.LookupSymbol("STRING"))
                },
                CurrentScope.LookupSymbol("STRING")));
            CurrentScope.DeclareSymbol(new BuiltinFunctionSymbol("LENGTH",
                new List<VariableSymbol>
                {
                    new VariableSymbol("inputString", CurrentScope.LookupSymbol("STRING"))
                },
                CurrentScope.LookupSymbol("INTEGER")));
            CurrentScope.DeclareSymbol(new BuiltinFunctionSymbol("STRTOINT",
                new List<VariableSymbol>
                {
                    new VariableSymbol("inputString", CurrentScope.LookupSymbol("STRING"))
                },
                CurrentScope.LookupSymbol("INTEGER")));
        }

        public ScopedSymbolTable CurrentScope { get; private set; }

        public void Analyze(AstNode<PascalTokenType> tree)
        {
            VisitTree(tree);
        }

        protected override void VisitProgramNode(ProgramNode<PascalTokenType> programNode)
        {
            CurrentScope.DeclareSymbol(new BuiltinSymbol(programNode.Name.Name));
            var globalScope =
                new ProcedureSymbol<PascalTokenType>("GLOBAL", null, new ScopedSymbolTable("GLOBAL", CurrentScope),
                    null);
            CurrentScope.DeclareSymbol(globalScope);
            CurrentScope = globalScope.SymbolTable;

            base.VisitProgramNode(programNode);

            CurrentScope = CurrentScope.ParentScope;
        }

        protected override void VisitVarDeclarationNode(VarDeclNode<PascalTokenType> varDeclNode)
        {
            // Get the type.
            var typeNode = varDeclNode.Type;
            if (typeNode == null)
            {
                // TODO Improve.
                throw new VariableDeclarationTypeMissingException(string.Join(",", varDeclNode.Variable),
                    varDeclNode.Token.LineNumber, varDeclNode.Token.ColumnNumber);
            }

            // Ensure type is declared.
            var type = CurrentScope.LookupSymbol(typeNode.Type);
            if (type == null)
            {
                throw new TypeNotDeclaredException(typeNode.Type);
            }

            // We can declare several variables with the same type in 1 declaration, so loop.
            foreach (var varName in varDeclNode.Variable)
            {
                var symbol = new VariableSymbol(varName, type);

                // Ensure that it has not been already declared.
                var existing = CurrentScope.LookupSymbolInThisTable(varName);
                if (existing != null)
                {
                    throw new SymbolAlreadyDeclaredException(varName);
                }

                CurrentScope.DeclareSymbol(symbol);
            }
        }

        protected override ValueResult VisitVariableNode(VarNode<PascalTokenType> varNode)
        {
            // Test that the symbol has been declared before usage.
            var symbol = CurrentScope.LookupSymbol(varNode.Name);

            if (symbol == null)
            {
                throw new SymbolNotDeclaredException(varNode.Name);
            }

            base.VisitVariableNode(varNode);

            return null;
        }

        protected override void VisitProcedureNode(ProcedureDeclarationNode<PascalTokenType> procedureDeclarationNode)
        {
            var parametersList = new List<VariableSymbol>();

            if (procedureDeclarationNode.Parameters != null)
            {
                foreach (var declaration in procedureDeclarationNode.Parameters)
                {
                    var type = CurrentScope.LookupSymbol(declaration.Type.Type);

                    foreach (var varName in declaration.Variable)
                    {
                        parametersList.Add(new VariableSymbol(varName, type));
                    }
                }
            }

            var procedureSymbol = new ProcedureSymbol<PascalTokenType>(
                procedureDeclarationNode.Name,
                parametersList,
                new ScopedSymbolTable(procedureDeclarationNode.Name, CurrentScope),
                procedureDeclarationNode.Block
            );

            CurrentScope.DeclareSymbol(procedureSymbol);
            CurrentScope = procedureSymbol.SymbolTable;

            base.VisitProcedureNode(procedureDeclarationNode);

            CurrentScope = CurrentScope.ParentScope;
        }

        protected override void VisitFunctionDeclarationNode(FunctionDeclarationNode<PascalTokenType> node)
        {
            var parametersList = new List<VariableSymbol>();

            if (node.Parameters != null)
            {
                foreach (var declaration in node.Parameters)
                {
                    var type = CurrentScope.LookupSymbol(declaration.Type.Type);

                    foreach (var varName in declaration.Variable)
                    {
                        parametersList.Add(new VariableSymbol(varName, type));
                    }
                }
            }

            var functionSymbol = new UserDefinedFunctionSymbol<PascalTokenType>(
                node.Name,
                parametersList,
                CurrentScope.LookupSymbol(node.Type.Type),
                node.Block,
                new ScopedSymbolTable(node.Name, CurrentScope)
            );

            CurrentScope.DeclareSymbol(functionSymbol);
            CurrentScope = functionSymbol.SymbolTable;

            CurrentScope.DeclareSymbol(new VariableSymbol(functionSymbol.Name, functionSymbol.Type));

            base.VisitFunctionDeclarationNode(node);

            CurrentScope = CurrentScope.ParentScope;
        }

        /// <summary>
        /// Tests that the procedure called is declared.
        /// Tests that the arguments are the same amount of as the definition.
        /// </summary>
        /// <param name="procedureCallNode"></param>
        protected override void VisitProcedureCallNode(ProcedureCallNode<PascalTokenType> procedureCallNode)
        {
            var symbol = CurrentScope.LookupSymbol(procedureCallNode.ProcedureName);

            if (symbol is ProcedureSymbolBase procedureSymbol)
            {
                if ((procedureSymbol.Parameters?.Count ?? 0) != procedureCallNode.Parameters.Count)
                {
                    throw new ProcedureCallWithIncorrectParameterCountException(
                        procedureCallNode.ProcedureName,
                        procedureCallNode.Token.LineNumber,
                        procedureCallNode.Token.ColumnNumber);
                }
            }
            else
            {
                throw new ProcedureNotDefinedException(
                    procedureCallNode.ProcedureName,
                    procedureCallNode.Token.LineNumber,
                    procedureCallNode.Token.ColumnNumber);
            }

            base.VisitProcedureCallNode(procedureCallNode);
        }

        /// <summary>
        /// Tests that the procedure called is declared.
        /// Tests that the arguments are the same amount of as the definition.
        /// </summary>
        /// <param name="functionCallNode"></param>
        protected override ValueResult VisitFunctionCallNode(FunctionCallNode<PascalTokenType> functionCallNode)
        {
            var symbol = CurrentScope.LookupSymbol(functionCallNode.FunctionName);

            if (symbol is FunctionSymbolBase procedureSymbol)
            {
                if ((procedureSymbol.Parameters?.Count ?? 0) != functionCallNode.Parameters.Count)
                {
                    // TODO: Function not defined?
                    throw new ProcedureCallWithIncorrectParameterCountException(
                        functionCallNode.FunctionName,
                        functionCallNode.Token.LineNumber,
                        functionCallNode.Token.ColumnNumber);
                }
            }
            else
            {
                // TODO: Function not defined?
                throw new ProcedureNotDefinedException(
                    functionCallNode.FunctionName,
                    functionCallNode.Token.LineNumber,
                    functionCallNode.Token.ColumnNumber);
            }

            base.VisitFunctionCallNode(functionCallNode);

            return null;
        }
    }
}