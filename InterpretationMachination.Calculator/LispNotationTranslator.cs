using System;
using InterpretationMachination.Calculator.AstNodes;
using InterpretationMachination.Interfaces.Interfaces;

namespace InterpretationMachination.Calculator
{
    public class LispNotationTranslator : IInterpreter<string>
    {
        public LispNotationTranslator()
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
                        $"({binOpNode.Token.Value} {InterpretAstNode(binOpNode.Left)} {InterpretAstNode(binOpNode.Right)})";
                case NumNode numNode:
                    return numNode.Value.ToString();
                case UnaryOpNode unaryOpNode:
                    return
                        $"({unaryOpNode.Token.Value} {InterpretAstNode(unaryOpNode.Factor)})";
                default:
                    throw new InvalidOperationException(
                        $"Node '{node}' is not of a known type and can't be interpreted.");
            }
        }
    }
}