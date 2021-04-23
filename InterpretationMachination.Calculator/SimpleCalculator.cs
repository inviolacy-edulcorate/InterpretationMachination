using System;
using InterpretationMachination.Calculator.AstNodes;
using InterpretationMachination.DataStructures.Tokens;
using InterpretationMachination.Interfaces.Interfaces;

namespace InterpretationMachination.Calculator
{
    public class SimpleCalculator : IInterpreter<int>
    {
        public SimpleCalculator()
        {
            Parser = new SimpleParser();
        }

        private SimpleParser Parser { get; }

        public int Interpret(string inputString)
        {
            var ast = Parser.Parse(inputString);

            return InterpretAstNode(ast);
        }

        private int InterpretAstNode(AstNode node)
        {
            switch (node)
            {
                case BinOpNode binOpNode:
                    switch (binOpNode.Token.Type)
                    {
                        case TokenType.OpAdd:
                            return InterpretAstNode(binOpNode.Left) + InterpretAstNode(binOpNode.Right);
                        case TokenType.OpSub:
                            return InterpretAstNode(binOpNode.Left) - InterpretAstNode(binOpNode.Right);
                        case TokenType.OpDiv:
                            return InterpretAstNode(binOpNode.Left) / InterpretAstNode(binOpNode.Right);
                        case TokenType.OpMul:
                            return InterpretAstNode(binOpNode.Left) * InterpretAstNode(binOpNode.Right);
                        default:
                            throw new InvalidOperationException(
                                $"BinOp contains the unknown type of '{binOpNode.Token.Type}'.");
                    }
                case NumNode numNode:
                    return numNode.Value;
                case UnaryOpNode unaryOpNode:
                    switch (unaryOpNode.Token.Type)
                    {
                        case TokenType.OpAdd:
                            return InterpretAstNode(unaryOpNode.Factor);
                        case TokenType.OpSub:
                            return - InterpretAstNode(unaryOpNode.Factor);
                        default:
                            throw new InvalidOperationException(
                                $"UnaryOp contains the unknown type of '{unaryOpNode.Token.Type}'.");
                    }
                default:
                    throw new InvalidOperationException(
                        $"Node '{node}' is not of a known type and can't be interpreted.");
            }
        }
    }
}