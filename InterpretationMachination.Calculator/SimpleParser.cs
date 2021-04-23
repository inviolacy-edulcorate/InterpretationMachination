using System;
using System.Collections.Generic;
using System.Linq;
using InterpretationMachination.Calculator.AstNodes;
using InterpretationMachination.DataStructures.Tokens;

namespace InterpretationMachination.Calculator
{
    public class SimpleParser
    {
        public SimpleParser()
        {
            Lexer = new SimpleLexer();
        }

        private SimpleLexer Lexer { get; }
        private GenericToken<TokenType> CurrentToken { get; set; }

        private void Eat(TokenType type)
        {
            if (CurrentToken.Type == type)
            {
                CurrentToken = Lexer.GetNextToken();
            }
            else
            {
                throw new InvalidOperationException("Can't read the expected type!");
            }
        }

        private void Eat(IEnumerable<TokenType> types)
        {
            if (types.Any(t => t == CurrentToken.Type))
            {
                CurrentToken = Lexer.GetNextToken();
            }
            else
            {
                throw new InvalidOperationException("Can't read any of the expected types!");
            }
        }

        /// <summary>
        /// Parse the "factor" rule.
        /// </summary>
        /// <returns></returns>
        private AstNode Factor()
        {
            AstNode factor;
            GenericToken<TokenType> token = CurrentToken;

            if (CurrentToken.Type == TokenType.Integer)
            {
                Eat(TokenType.Integer);

                factor = new NumNode
                {
                    Token = token,
                    Value = token.ValueAsInt
                };
            }
            // Unary Operators
            else if (
                CurrentToken.Type == TokenType.OpAdd ||
                CurrentToken.Type == TokenType.OpSub
            )
            {
                Eat(new List<TokenType>
                {
                    TokenType.OpAdd,
                    TokenType.OpSub,
                });

                factor = new UnaryOpNode()
                {
                    Token = token,
                    Factor = Pterm(),
                };
            }
            else
            {
                throw new InvalidOperationException();
            }

            return factor;
        }

        /// <summary>
        /// Parse the "pterm" rule.
        /// </summary>
        /// <returns></returns>
        private AstNode Pterm()
        {
            if (CurrentToken.Type == TokenType.ParL)
            {
                Eat(TokenType.ParL);
                var expr = Expr();
                Eat(TokenType.ParR);
                return expr;
            }
            else
            {
                return Factor();
            }
        }

        private AstNode Term()
        {
            var result = Pterm();

            while (
                CurrentToken.Type == TokenType.OpMul ||
                CurrentToken.Type == TokenType.OpDiv
            )
            {
                var opToken = CurrentToken;
                Eat(new List<TokenType>
                {
                    TokenType.OpMul,
                    TokenType.OpDiv,
                });

                var right = Pterm();

                var opResult = new BinOpNode
                {
                    Left = result,
                    Right = right,
                    Token = opToken,
                };

                result = opResult;
            }

            return result;
        }

        private AstNode Expr()
        {
            var result = Term();

            while (
                CurrentToken.Type == TokenType.OpAdd ||
                CurrentToken.Type == TokenType.OpSub
            )
            {
                var opToken = CurrentToken;
                Eat(new List<TokenType>
                {
                    TokenType.OpAdd,
                    TokenType.OpSub,
                });

                var right = Term();

                var opResult = new BinOpNode
                {
                    Left = result,
                    Right = right,
                    Token = opToken,
                };

                result = opResult;
            }

            return result;
        }

        public AstNode Parse(string inputString)
        {
            Lexer.InputString(inputString);

            CurrentToken = Lexer.GetNextToken();

            return Expr();
        }
    }
}