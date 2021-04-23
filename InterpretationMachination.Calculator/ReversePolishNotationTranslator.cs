using System;
using InterpretationMachination.Calculator.AstNodes;
using InterpretationMachination.DataStructures.Tokens;
using InterpretationMachination.Interfaces.Interfaces;

namespace InterpretationMachination.Calculator
{
    public class ReversePolishNotationTranslator : IInterpreter<string>
    {
        public ReversePolishNotationTranslator()
        {
            Parser = new SimpleParser();
        }

        private SimpleParser Parser { get; set; }

        public string Interpret(string inputString)
        {
            var ast = Parser.Parse(inputString);

            return InterpretAstNode(ast);
        }

        private string InterpretAstNode(AstNode node)
        {
            switch (node)
            {
                case BinOpNode binOpNode:
                    return
                        $"{InterpretAstNode(binOpNode.Left)} {InterpretAstNode(binOpNode.Right)} {binOpNode.Token.Value}";
                case NumNode numNode:
                    return numNode.Value.ToString();
                case UnaryOpNode unaryOpNode:
                    switch (unaryOpNode.Token.Type)
                    {
                        case TokenType.OpAdd:
                            return
                                $"{InterpretAstNode(unaryOpNode.Factor)}";
                        case TokenType.OpSub:
                            return
                                $"{InterpretAstNode(unaryOpNode.Factor)} NEGATE";
                        default:
                            throw new InvalidOperationException(
                                $"TokenType '{unaryOpNode.Token.Type}' is not supported for unary operator.");
                    }
                default:
                    throw new InvalidOperationException(
                        $"Node '{node}' is not of a known type and can't be interpreted.");
            }
        }
    }
}