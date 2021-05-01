using System;
using InterpretationMachination.DataStructures.SymbolTable;
using InterpretationMachination.DataStructures.SymbolTable.Symbols;
using InterpretationMachination.PascalInterpreter.AstNodes;

namespace InterpretationMachination.PascalInterpreter
{
    public class Source2SourceCompiler : AstVisitor<PascalTokenType>
    {
        public Source2SourceCompiler()
        {
            SemanticAnalyzer = new SemanticAnalyzer();
            Parser = new SimplePascalParser();
        }

        public string Output { get; set; }
        public SemanticAnalyzer SemanticAnalyzer { get; private set; }
        public ScopedSymbolTable CurrentScope { get; private set; }
        public SimplePascalParser Parser { get; set; }

        public void Compile(string input)
        {
            var ast = Parser.Parse(input);

            SemanticAnalyzer.Analyze(ast);
            CurrentScope = SemanticAnalyzer.CurrentScope;

            VisitTree(ast);
        }

        protected override void VisitProcedureNode(ProcedureDeclarationNode<PascalTokenType> procedureDeclarationNode)
        {
            // Procedure Name.
            Output += $"{Indent()}procedure {procedureDeclarationNode.Name}{CurrentScope.Depth}";

            // Move scope down.
            CurrentScope = ((ProcedureSymbol<PascalTokenType>) CurrentScope.LookupSymbol(procedureDeclarationNode.Name)).SymbolTable;

            // Procedure Parameters.
            if (procedureDeclarationNode.Parameters != null)
            {
                Output += $"(";

                for (var index = 0; index < procedureDeclarationNode.Parameters.Count; index++)
                {
                    var procedureNodeParameter = procedureDeclarationNode.Parameters[index];

                    ProcedureParameterDeclaration(procedureNodeParameter);

                    if (index + 1 < procedureDeclarationNode.Parameters.Count)
                    {
                        Output += ";";
                    }
                }

                Output += $")";
            }

            // Close initial declaration procedure.
            Output += $";{Environment.NewLine}";

            // Process internal of procedure.
            VisitAstNode(procedureDeclarationNode.Block);

            // Move scope back up.
            CurrentScope = CurrentScope.ParentScope;
        }

        protected override void VisitProgramNode(ProgramNode<PascalTokenType> programNode)
        {
            Output += $"program {programNode.Name.Name}{CurrentScope.Depth}";

            Output += $";{Environment.NewLine}";

            CurrentScope = ((ProcedureSymbol<PascalTokenType>) CurrentScope.LookupSymbol("GLOBAL")).SymbolTable;

            // Block.
            foreach (var blockNodeDeclaration in programNode.Block.Declarations)
            {
                VisitAstNode(blockNodeDeclaration);
            }

            Output += $"{Environment.NewLine}{Indent(CurrentScope.Depth - 1)}begin{Environment.NewLine}";

            foreach (var compoundNodeChild in programNode.Block.CompoundStatement.Children)
            {
                VisitAstNode(compoundNodeChild);

                if (!(compoundNodeChild is NoOpNode<PascalTokenType> _))
                {
                    Output += $";";
                }

                if (compoundNodeChild is NoOpNode<PascalTokenType> _)
                {
                    Output += $"{Environment.NewLine}";
                }
            }

            Output += $"{Indent(CurrentScope.Depth - 1)}end. {{END OF {programNode.Name.Name}}}";

            // Exit block.
            CurrentScope = CurrentScope.ParentScope;
        }

        protected override ValueResult VisitUnaryOpNode(UnaryOpNode<PascalTokenType> unaryOpNode)
        {
            base.VisitUnaryOpNode(unaryOpNode);

            return null;
        }

        protected override void VisitCompoundNode(CompoundNode<PascalTokenType> compoundNode)
        {
            Output += $"{Environment.NewLine}{Indent(CurrentScope.Depth - 1)}begin{Environment.NewLine}";

            foreach (var compoundNodeChild in compoundNode.Children)
            {
                VisitAstNode(compoundNodeChild);

                if (!(compoundNodeChild is NoOpNode<PascalTokenType> _))
                {
                    Output += $";";
                }

                if (compoundNodeChild is NoOpNode<PascalTokenType> _)
                {
                    Output += $"{Environment.NewLine}";
                }
            }

            Output += $"{Indent(CurrentScope.Depth - 1)}end; {{END OF {CurrentScope.Name}}}{Environment.NewLine}";
        }

        protected override void VisitAssignNode(AssignNode<PascalTokenType> assignNode)
        {
            Output += $"{Indent()}";

            VisitAstNode(assignNode.Variable);

            Output += $" := ";

            VisitAstNode(assignNode.Expr);
        }

        protected override ValueResult VisitVariableNode(VarNode<PascalTokenType> varNode)
        {
            Output +=
                $"<{varNode.Name}{CurrentScope.LookupSymbol(varNode.Name).ScopeLevel}:{CurrentScope.LookupSymbol(varNode.Name).Type.Name}>";

            base.VisitVariableNode(varNode);

            return null;
        }

        protected override void VisitNoOpNode(NoOpNode<PascalTokenType> noOpNode)
        {
            base.VisitNoOpNode(noOpNode);
        }

        protected override void VisitBlockNode(BlockNode<PascalTokenType> blockNode)
        {
            base.VisitBlockNode(blockNode);
        }

        protected override void VisitVarDeclarationNode(VarDeclNode<PascalTokenType> varDeclNode)
        {
            foreach (var name in varDeclNode.Variable)
            {
                Output +=
                    $"{Indent()}var {name}{CurrentScope.Depth} : {varDeclNode.Type.Type}{CurrentScope.LookupSymbol(varDeclNode.Type.Type).ScopeLevel};{Environment.NewLine}";
            }

            base.VisitVarDeclarationNode(varDeclNode);
        }

        protected override void VisitTypeNode(TypeNode<PascalTokenType> typeNode)
        {
            base.VisitTypeNode(typeNode);
        }

        protected override ValueResult VisitBinOpNode(BinOpNode<PascalTokenType> node)
        {
            VisitAstNode(node.Left);
            switch (node.Token.Type)
            {
                case PascalTokenType.OpAdd:
                    Output += " + ";
                    break;
                case PascalTokenType.OpSub:
                    Output += " - ";
                    break;
                case PascalTokenType.OpDiv:
                    Output += " / ";
                    break;
                case PascalTokenType.OpIntDiv:
                    Output += " div ";
                    break;
                case PascalTokenType.OpMul:
                    Output += " * ";
                    break;
                default:
                    throw new InvalidOperationException(
                        $"BinOp contains the unknown type of '{node.Token.Type}'.");
            }

            VisitAstNode(node.Right);

            return null;
        }

        protected override ValueResult VisitNumericNode(LiteralNode<PascalTokenType> node)
        {
            base.VisitNumericNode(node);

            return null;
        }

        private void ProcedureParameterDeclaration(VarDeclNode<PascalTokenType> varDeclNode)
        {
            foreach (var name in varDeclNode.Variable)
            {
                Output +=
                    $"{name}{CurrentScope.Depth} : {varDeclNode.Type.Type}{CurrentScope.LookupSymbol(varDeclNode.Type.Type).ScopeLevel}";
            }

            base.VisitVarDeclarationNode(varDeclNode);
        }

        private string Indent(int depth)
        {
            string o = "";

            while (depth > 0)
            {
                depth -= 1;
                o += "   ";
            }

            return o;
        }

        private string Indent()
        {
            return Indent(CurrentScope.Depth);
        }
    }
}