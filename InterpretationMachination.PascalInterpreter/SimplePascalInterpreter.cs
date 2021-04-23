using System;
using System.IO;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;
using InterpretationMachination.DataStructures.CallStack;
using InterpretationMachination.DataStructures.SymbolTable;
using InterpretationMachination.Interfaces.Interfaces;
using InterpretationMachination.PascalInterpreter.AstNodes;

namespace InterpretationMachination.PascalInterpreter
{
    public class SimplePascalInterpreter : AstVisitor<PascalTokenType>, IInterpreter
    {
        public SimplePascalInterpreter()
        {
            GlobalScope = new BasicCallStack();
            Parser = new SimplePascalParser();
            SymbolTableBuilder = new SemanticAnalyzer();
        }

        public BasicCallStack GlobalScope { get; }

        private IParser<PascalTokenType> Parser { get; }

        private SemanticAnalyzer SymbolTableBuilder { get; }

        public void Interpret(string inputString)
        {
            var ast = Parser.Parse(inputString);

            SymbolTableBuilder.VisitTree(ast);

            VisitAstNode(ast);
        }

        protected override void VisitProgramNode(ProgramNode<PascalTokenType> programNode)
        {
            var y = (SymbolTableBuilder.CurrentScope.LookupSymbol("GLOBAL") as ProcedureSymbol<PascalTokenType>)
                ?.SymbolTable;
            var x = new StackFrame(y, "PROGRAM", 1);
            GlobalScope.Push(x);

            base.VisitProgramNode(programNode);

            // TODO Reintroduce when we can take output in a different way. Only way currently is "memory dump". For testing.
            //GlobalScope.Pop();
        }

        protected override object VisitBinOpNode(BinOpNode<PascalTokenType> binOpNode)
        {
            // Handle Bools first.
            if (binOpNode.Token.Type == PascalTokenType.Equals)
            {
                return VisitAstNode(binOpNode.Left).ToString() == VisitAstNode(binOpNode.Right).ToString();
            }

            var la = InterpretAstNodeAsDouble(binOpNode.Left);
            var ra = InterpretAstNodeAsDouble(binOpNode.Right);

            switch (binOpNode.Token.Type)
            {
                case PascalTokenType.OpAdd:
                    return la + ra;
                case PascalTokenType.OpSub:
                    return la - ra;
                case PascalTokenType.OpDiv:
                    return la / ra;
                case PascalTokenType.OpIntDiv:
                    return Math.Floor(la / ra);
                case PascalTokenType.OpMul:
                    return la * ra;
                default:
                    throw new InvalidOperationException(
                        $"BinOp contains the unknown type of '{binOpNode.Token.Type}'.");
            }
        }

        protected override object VisitNumericNode(LiteralNode<PascalTokenType> node)
        {
            return node.Value;
        }

        protected override object VisitVariableNode(VarNode<PascalTokenType> varNode)
        {
            return GlobalScope[varNode.Name];
        }

        protected override object VisitUnaryOpNode(UnaryOpNode<PascalTokenType> unaryOpNode)
        {
            switch (unaryOpNode.Token.Type)
            {
                case PascalTokenType.OpAdd:
                    return InterpretAstNodeAsDouble(unaryOpNode.Factor);
                case PascalTokenType.OpSub:
                    return -(InterpretAstNodeAsDouble(unaryOpNode.Factor));
                default:
                    throw new InvalidOperationException(
                        $"UnaryOp contains the unknown type of '{unaryOpNode.Token.Type}'.");
            }
        }

        protected override void VisitAssignNode(AssignNode<PascalTokenType> assignNode)
        {
            GlobalScope[assignNode.Variable.Name] = VisitAstNode(assignNode.Expr);
        }

        protected override void VisitProcedureNode(ProcedureDeclarationNode<PascalTokenType> procedureDeclarationNode)
        {
            // Do nothing, declaration has happened, no need to run now.
        }

        protected override void VisitFunctionDeclarationNode(FunctionDeclarationNode<PascalTokenType> node)
        {
            // Do nothing, declaration has happened, no need to run now.
        }

        protected override void VisitProcedureCallNode(ProcedureCallNode<PascalTokenType> procedureCallNode)
        {
            var lookupSymbol = GlobalScope.Top.SymbolTable.LookupSymbol(procedureCallNode.ProcedureName);
            if (!(lookupSymbol is ProcedureSymbol<PascalTokenType> procedureSymbol))
            {
                if (lookupSymbol is BuiltinProcedureSymbol builtin)
                {
                    switch (builtin.Name)
                    {
                        case "WRITELN":
                            // TODO: Replace with function.
                            Console.WriteLine("WRITELN: " + VisitAstNode(procedureCallNode.Parameters[0]));
                            break;
                        default:
                            // TODO: Better exception.
                            throw new NotImplementedException();
                    }

                    return;
                }

                // TODO : Replace with meaningful message.
                throw new InvalidOperationException();
            }

            var stackFrame = new StackFrame(procedureSymbol.SymbolTable, procedureSymbol.Name,
                GlobalScope.Top.Level + 1);

            for (var i = 0; i < procedureSymbol.Parameters.Count; i++)
            {
                var parameterSymbol = procedureSymbol.Parameters[i];
                stackFrame[parameterSymbol.Name] = VisitAstNode(procedureCallNode.Parameters[i]);
            }

            GlobalScope.Push(stackFrame);

            VisitAstNode(procedureSymbol.ProcedureBody);

            GlobalScope.Pop();
        }

        protected override object VisitFunctionCallNode(FunctionCallNode<PascalTokenType> node)
        {
            var lookupSymbol = GlobalScope.Top.SymbolTable.LookupSymbol(node.FunctionName);

            // If the symbol is NOT user defined, then handle the built-in.
            if (!(lookupSymbol is UserDefinedFunctionSymbol<PascalTokenType> functionSymbol))
            {
                if (lookupSymbol is BuiltinFunctionSymbol builtin)
                {
                    switch (builtin.Name)
                    {
                        case "LENGTH":
                            var str = VisitAstNode(node.Parameters[0]);
                            if (str is string strstr)
                            {
                                return strstr.Length;
                            }

                            // TODO: Length param String is not string.
                            throw new InvalidOperationException();

                        case "READFILE":
                            var path = VisitAstNode(node.Parameters[0]);
                            if (path is string pathStr)
                            {
                                return File.ReadAllText(pathStr);
                            }

                            // TODO: Length param String is not string.
                            throw new InvalidOperationException();

                        default:
                            // TODO: Better exception.
                            throw new NotImplementedException();
                    }
                }

                // TODO : Replace with meaningful message.
                throw new InvalidOperationException();
            }

            var stackFrame = new StackFrame(functionSymbol.SymbolTable, functionSymbol.Name, GlobalScope.Top.Level + 1);

            for (var i = 0; i < functionSymbol.Parameters.Count; i++)
            {
                var parameterSymbol = functionSymbol.Parameters[i];
                stackFrame[parameterSymbol.Name] = VisitAstNode(node.Parameters[i]);
            }

            GlobalScope.Push(stackFrame);

            VisitAstNode(functionSymbol.FunctionBody);

            var result = GlobalScope.Pop()[functionSymbol.Name];

            return result;
        }

        protected override void VisitIfThenNode(IfThenNode<PascalTokenType> ifThenNode)
        {
            // Evaluate the condition.
            var condition = (bool) VisitAstNode(ifThenNode.Condition);

            // If true, run the Then.
            if (condition)
            {
                VisitAstNode(ifThenNode.Then);
            }
            else
            {
                // Otherwise, if there is an else, run it.
                if (ifThenNode.Else != null)
                {
                    VisitAstNode(ifThenNode.Else);
                }
            }
        }

        protected override object VisitIndexNode(IndexNode<PascalTokenType> indexNode)
        {
            var variable = VisitVariableNode(indexNode.Variable);
            var index = VisitAstNode(indexNode.Expr);

            if (variable is string str)
            {
                int i;
                if (index is double d)
                {
                    i = (int) d;
                }
                else if (index is int iInt)
                {
                    i = iInt;
                }
                else
                {
                    // TODO: index is not integer.
                    throw new InvalidOperationException();
                }

                // Convert to string before returning.
                return str[i].ToString();
            }

            // TODO: variable is not supported indexing, or index is not integer.
            throw new InvalidOperationException();
        }

        protected override void VisitWhileDoNode(WhileDoNode<PascalTokenType> whileDoNode)
        {
            // If true, run the Then.
            while ((bool) VisitAstNode(whileDoNode.Condition))
            {
                VisitAstNode(whileDoNode.Statement);
            }
        }

        private double InterpretAstNodeAsDouble(AstNode<PascalTokenType> node)
        {
            object v = VisitAstNode(node);
            double? vd = v as double?;
            int? vi = v as int?;
            return vd ?? Convert.ToDouble(vi.Value);
        }
    }
}