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

        protected override ValueResult VisitBinOpNode(BinOpNode<PascalTokenType> binOpNode)
        {
            var left = VisitAstNode(binOpNode.Left);
            var right = VisitAstNode(binOpNode.Right);

            // Booleans first.
            if (binOpNode.Token.Type == PascalTokenType.Equals)
            {
                return CreateBooleanValueResult(
                    left.Value.ToString() == right.Value.ToString()
                );
            }

            if (binOpNode.Token.Type == PascalTokenType.GreaterThan)
            {
                if (left.Type.Name == "INTEGER" && right.Type.Name == "INTEGER")
                {
                    return CreateBooleanValueResult(
                        ((int) left.Value) > ((int) right.Value)
                    );
                }

                if (left.Type.Name == "REAL" && right.Type.Name == "REAL")
                {
                    return CreateBooleanValueResult(
                        ((double) left.Value) > ((double) right.Value)
                    );
                }

                if (left.Type.Name == "INTEGER" && right.Type.Name == "REAL")
                {
                    Console.WriteLine(left.Value.GetType());
                    return CreateBooleanValueResult(
                        ((int) left.Value) > ((double) right.Value)
                    );
                }

                if (left.Type.Name == "REAL" && right.Type.Name == "INTEGER")
                {
                    return CreateBooleanValueResult(
                        ((double) left.Value) > ((int) right.Value)
                    );
                }

                // TODO: Invalid < or > comparison types
                throw new InvalidOperationException();
            }

            if (binOpNode.Token.Type == PascalTokenType.LessThan)
            {
                if (left.Type.Name == "INTEGER" && right.Type.Name == "INTEGER")
                {
                    return CreateBooleanValueResult(
                        ((int) left.Value) < ((int) right.Value)
                    );
                }

                if (left.Type.Name == "REAL" && right.Type.Name == "REAL")
                {
                    return CreateBooleanValueResult(
                        ((double) left.Value) < ((double) right.Value)
                    );
                }

                if (left.Type.Name == "INTEGER" && right.Type.Name == "REAL")
                {
                    return CreateBooleanValueResult(
                        ((int) left.Value) < ((double) right.Value)
                    );
                }

                if (left.Type.Name == "REAL" && right.Type.Name == "INTEGER")
                {
                    return CreateBooleanValueResult(
                        ((double) left.Value) < ((int) right.Value)
                    );
                }

                // TODO: Invalid < or > comparison types
                throw new InvalidOperationException();
            }

            if (left.Type.Name == "STRING" || right.Type.Name == "STRING")
            {
                var ld = left.Value.ToString();
                var rd = right.Value.ToString();

                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (binOpNode.Token.Type)
                {
                    case PascalTokenType.OpAdd:
                        return CreateStringValueResult(ld + rd);
                    default:
                        throw new InvalidOperationException(
                            $"BinOp contains the unknown type of '{binOpNode.Token.Type}' for types '{left.Type.Name}' and '{right.Type.Name}'.");
                }
            }

            // If we're dealing with reals anywhere, make the result a real as well.
            if (left.Type.Name == "REAL" || right.Type.Name == "REAL")
            {
                var ld = Convert.ToDouble(left.Value);
                var rd = Convert.ToDouble(right.Value);

                double result;

                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (binOpNode.Token.Type)
                {
                    case PascalTokenType.OpAdd:
                        result = ld + rd;
                        break;
                    case PascalTokenType.OpSub:
                        result = ld - rd;
                        break;
                    case PascalTokenType.OpDiv:
                        result = ld / rd;
                        break;
                    case PascalTokenType.OpIntDiv:
                        result = Math.Floor(ld / rd);
                        break;
                    case PascalTokenType.OpMul:
                        result = ld * rd;
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"BinOp contains the unknown type of '{binOpNode.Token.Type}'.");
                }

                return CreateRealValueResult(result);
            }

            if (left.Type.Name == "INTEGER" || right.Type.Name == "INTEGER")
            {
                var ld = Convert.ToInt32(left.Value);
                var rd = Convert.ToInt32(right.Value);

                int result;

                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (binOpNode.Token.Type)
                {
                    case PascalTokenType.OpAdd:
                        result = ld + rd;
                        break;
                    case PascalTokenType.OpSub:
                        result = ld - rd;
                        break;
                    case PascalTokenType.OpDiv:
                        // TODO: Breaking the pattern here, because int/int can result in double.
                        return CreateRealValueResult((double) ld / rd);
                    case PascalTokenType.OpIntDiv:
                        result = (int) Math.Floor((double) ld / rd);
                        break;
                    case PascalTokenType.OpMul:
                        result = ld * rd;
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"BinOp contains the unknown type of '{binOpNode.Token.Type}'.");
                }

                return CreateIntegerValueResult(result);
            }

            // TODO: No operation types applied.
            throw new InvalidOperationException(
                $"No binary operation can be applied on {left.Type.Name} and {right.Type.Name}");
        }

        protected override ValueResult VisitNumericNode(LiteralNode<PascalTokenType> node)
        {
            return CreateValueResult(node.Type, node.Value);
        }

        protected override ValueResult VisitVariableNode(VarNode<PascalTokenType> varNode)
        {
            var type = GlobalScope.Top.SymbolTable.LookupSymbol(varNode.Name);

            return CreateValueResult(type.Type.Name, GlobalScope[varNode.Name]);
        }

        protected override ValueResult VisitUnaryOpNode(UnaryOpNode<PascalTokenType> unaryOpNode)
        {
            var val = VisitAstNode(unaryOpNode.Factor);
            switch (val.Value)
            {
                case int v:
                    return unaryOpNode.Token.Type switch
                    {
                        PascalTokenType.OpAdd => VisitAstNode(unaryOpNode.Factor),
                        PascalTokenType.OpSub => CreateValueResult(val.Type.Name, -v),
                        _ => throw new InvalidOperationException(
                            $"UnaryOp contains the unknown type of '{unaryOpNode.Token.Type}'.")
                    };
                case double v:
                    return unaryOpNode.Token.Type switch
                    {
                        PascalTokenType.OpAdd => VisitAstNode(unaryOpNode.Factor),
                        PascalTokenType.OpSub => CreateValueResult(val.Type.Name, -v),
                        _ => throw new InvalidOperationException(
                            $"UnaryOp contains the unknown type of '{unaryOpNode.Token.Type}'.")
                    };
                default:
                    throw new InvalidOperationException(
                        $"Unary Operation can't be applied to type {val.Type.Name}");
            }
        }

        protected override void VisitAssignNode(AssignNode<PascalTokenType> assignNode)
        {
            GlobalScope[assignNode.Variable.Name] = VisitAstNode(assignNode.Expr).Value;
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
                            Console.WriteLine("WRITELN: " + VisitAstNode(procedureCallNode.Parameters[0]).Value);
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

            var stackFrame = new StackFrame(
                procedureSymbol.SymbolTable,
                procedureSymbol.Name,
                GlobalScope.Top.Level + 1);

            for (var i = 0; i < procedureSymbol.Parameters.Count; i++)
            {
                var parameterSymbol = procedureSymbol.Parameters[i];
                stackFrame[parameterSymbol.Name] = VisitAstNode(procedureCallNode.Parameters[i]).Value;
            }

            GlobalScope.Push(stackFrame);

            VisitAstNode(procedureSymbol.ProcedureBody);

            GlobalScope.Pop();
        }

        protected override ValueResult VisitFunctionCallNode(FunctionCallNode<PascalTokenType> node)
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
                            if (str.Type.Name == "STRING")
                            {
                                return CreateIntegerValueResult(str.Value.ToString().Length);
                            }

                            // TODO: Length param String is not string.
                            throw new InvalidOperationException();

                        case "READFILE":
                            var path = VisitAstNode(node.Parameters[0]);
                            if (path.Type.Name == "STRING" && path.Value is string pathStr)
                            {
                                return CreateStringValueResult(File.ReadAllText(pathStr));
                            }

                            // TODO: Length param String is not string.
                            throw new InvalidOperationException();
                        case "STRTOINT":
                            var strToConvert = VisitAstNode(node.Parameters[0]);
                            return CreateIntegerValueResult(Convert.ToInt32(strToConvert.Value)); 
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
                stackFrame[parameterSymbol.Name] = VisitAstNode(node.Parameters[i]).Value;
            }

            GlobalScope.Push(stackFrame);

            VisitAstNode(functionSymbol.FunctionBody);

            GlobalScope.Pop();
            var result = stackFrame[functionSymbol.Name];
            var resultType = functionSymbol.TypeSymbol;

            return new ValueResult
            {
                Value = result,
                Type = resultType
            };
        }

        protected override void VisitIfThenNode(IfThenNode<PascalTokenType> ifThenNode)
        {
            // Evaluate the condition.
            var conditionValue = VisitAstNode(ifThenNode.Condition);
            if (conditionValue.Type.Name != "BOOLEAN")
            {
                // TODO TYpe of condition is not boolean when it should be.
                throw new InvalidOperationException();
            }

            var condition = (bool) conditionValue.Value;

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

        protected override ValueResult VisitIndexNode(IndexNode<PascalTokenType> indexNode)
        {
            var variable = VisitVariableNode(indexNode.Variable);
            var index = VisitAstNode(indexNode.Expr);

            if (variable.Type.Name == "STRING")
            {
                int i;
                if (index.Type.Name == "INTEGER")
                {
                    i = Convert.ToInt32(index.Value);
                }
                else
                {
                    // TODO: index is not integer. Only INT indexing is allowed currently.
                    throw new InvalidOperationException();
                }

                // Convert to string before returning.
                return CreateStringValueResult(((string) variable.Value)[i].ToString());
            }

            // TODO: variable is not supported indexing, or index is not integer.
            throw new InvalidOperationException();
        }

        protected override void VisitWhileDoNode(WhileDoNode<PascalTokenType> whileDoNode)
        {
            // If true, run the Then.
            while (true)
            {
                var conditionResult = VisitAstNode(whileDoNode.Condition);
                if (conditionResult.Type.Name != "BOOLEAN")
                {
                    // TODO: Only boolean conditions allowed.
                    throw new InvalidOperationException();
                }

                // Condition false? break;
                if (!(bool) conditionResult.Value)
                {
                    break;
                }

                VisitAstNode(whileDoNode.Statement);
            }
        }

        private ValueResult CreateBooleanValueResult(bool value)
            => CreateValueResult("BOOLEAN", value);

        private ValueResult CreateRealValueResult(double value)
            => CreateValueResult("REAL", value);

        private ValueResult CreateStringValueResult(string value)
            => CreateValueResult("STRING", value);

        private ValueResult CreateIntegerValueResult(int value)
            => CreateValueResult("INTEGER", value);

        private ValueResult CreateValueResult(string type, object value)
        {
            return new ()
            {
                Type = GlobalScope.Top.SymbolTable.LookupSymbol(type),
                Value = value
            };
        }
    }
}