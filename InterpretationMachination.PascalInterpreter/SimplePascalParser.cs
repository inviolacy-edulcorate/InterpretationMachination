using System;
using System.Collections.Generic;
using System.Linq;
using IM.Lexers;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;
using InterpretationMachination.DataStructures.SymbolTable.Symbols;
using InterpretationMachination.DataStructures.Tokens;
using InterpretationMachination.Interfaces.Exceptions;
using InterpretationMachination.Interfaces.Interfaces;
using InterpretationMachination.PascalInterpreter.AstNodes;

namespace InterpretationMachination.PascalInterpreter
{
    public class SimplePascalParser : IParser<PascalTokenType>
    {
        private IM.Lexers.Interfaces.ILexer<PascalTokenType> Lexer { get; set; }

        private GenericToken<PascalTokenType> CurrentToken { get; set; }

        private TokenSet<PascalTokenType> TokenSet
        {
            get
            {
                var ts = new TokenSet<PascalTokenType>
                {
                    ["."] = PascalTokenType.Dot,
                    [";"] = PascalTokenType.SemCol,
                    [":="] = PascalTokenType.OpAssign,
                    ["+"] = PascalTokenType.OpAdd,
                    ["-"] = PascalTokenType.OpSub,
                    ["*"] = PascalTokenType.OpMul,
                    ["("] = PascalTokenType.ParL,
                    [")"] = PascalTokenType.ParR,
                    ["/"] = PascalTokenType.OpDiv,
                    [":"] = PascalTokenType.Colon,
                    [","] = PascalTokenType.Comma,
                    ["="] = PascalTokenType.Equals,
                    ["["] = PascalTokenType.Brl,
                    ["]"] = PascalTokenType.Brr,
                    ["<"] = PascalTokenType.LessThan,
                    [">"] = PascalTokenType.GreaterThan,
                    ["#"] = PascalTokenType.HashTag,
                    [".."] = PascalTokenType.DoubleDot,
                };
                ts.IntegerTypes.Add(PascalTokenType.ConstInteger);
                ts.RealTypes.Add(PascalTokenType.ConstReal);
                ts.StringTypes.Add(PascalTokenType.Id);
                ts.StringTypes.Add(PascalTokenType.ConstString);
                ts.BooleanTypes.Add(PascalTokenType.KwFalse);
                ts.BooleanTypes.Add(PascalTokenType.KwTrue);

                ts.NumericRecognizePattern = @"[0-9]";
                ts.NumericPattern = @"[0-9.]";
                ts.IdRecognizePattern = @"[a-zA-Z_]";
                ts.IdPattern = @"[a-zA-Z0-9_]";

                ts.WhitespaceCharacters.Add("\n");
                ts.WhitespaceCharacters.Add("\r");
                ts.WhitespaceCharacters.Add("\t");
                ts.WhitespaceCharacters.Add(" ");

                ts.NewLineCharacters.Add("\n");

                ts.CommentStartEndCharacters["{"] = "}";

                ts.EndOfStreamTokenType = PascalTokenType.EndOfFile;
                ts.IdTokenType = PascalTokenType.Id;
                ts.DoubleTokenType = PascalTokenType.ConstReal;
                ts.IntegerTokenType = PascalTokenType.ConstInteger;
                ts.StringTokenType = PascalTokenType.ConstString;

                ts.StringStartCharacter = "'";

                return ts;
            }
        }

        private Dictionary<string, PascalTokenType> KeyWords => new Dictionary<string, PascalTokenType>
        {
            {"BEGIN", PascalTokenType.KwBegin},
            {"END", PascalTokenType.KwEnd},
            {"DIV", PascalTokenType.OpIntDiv},
            {"PROGRAM", PascalTokenType.KwProgram},
            {"REAL", PascalTokenType.TypeReal},
            {"INTEGER", PascalTokenType.TypeInteger},
            {"STRING", PascalTokenType.TypeString},
            {"VAR", PascalTokenType.KwVar},
            {"PROCEDURE", PascalTokenType.KwProcedure},
            {"FUNCTION", PascalTokenType.KwFunction},
            {"IF", PascalTokenType.KwIf},
            {"THEN", PascalTokenType.KwThen},
            {"ELSE", PascalTokenType.KwElse},
            {"TRUE", PascalTokenType.KwTrue},
            {"FALSE", PascalTokenType.KwFalse},
            {"BOOLEAN", PascalTokenType.TypeBoolean},
            {"WHILE", PascalTokenType.KwWhile},
            {"DO", PascalTokenType.KwDo},
            {"ARRAY", PascalTokenType.KwArray},
            {"OF", PascalTokenType.KwOf},
        };

        public AstNode<PascalTokenType> Parse(string inputString)
        {
            // TODO: New Lexer
            Lexer = new Lexer<PascalTokenType>(inputString, TokenSet, KeyWords);

            return Parse();
        }

        private AstNode<PascalTokenType> Parse()
        {
            CurrentToken = Lexer.GetNextToken();
            return Program();
        }

        private void Eat(PascalTokenType type)
        {
            if (CurrentToken.Type == type)
            {
                CurrentToken = Lexer.GetNextToken();
            }
            else
            {
                throw new UnexpectedTokenTypeException(
                    type,
                    CurrentToken.Type,
                    CurrentToken.LineNumber,
                    CurrentToken.ColumnNumber);
            }
        }

        private void Eat(params PascalTokenType[] types)
        {
            if (types.Any(t => t == CurrentToken.Type))
            {
                CurrentToken = Lexer.GetNextToken();
            }
            else
            {
                // TODO: Can't reach with testing.
                throw new InvalidOperationException("Can't read any of the expected types!");
            }
        }

        private AstNode<PascalTokenType> Program()
        {
            var programToken = new ProgramNode<PascalTokenType>
            {
                Token = CurrentToken
            };

            Eat(PascalTokenType.KwProgram);

            programToken.Name = Variable();

            Eat(PascalTokenType.SemCol);

            programToken.Block = Block();

            Eat(PascalTokenType.Dot);

            return programToken;
        }

        private BlockNode<PascalTokenType> Block()
        {
            var node = new BlockNode<PascalTokenType>
            {
                Token = CurrentToken
            };

            node.Declarations = Declarations();

            node.CompoundStatement = CompoundStatement();

            return node;
        }

        /// <summary>
        /// declarations : VAR (variable-declaration SEMCOL)+ (procedure-declarations | function-declarations)*
        ///              | empty
        /// </summary>
        /// <returns></returns>
        private List<AstNode<PascalTokenType>> Declarations()
        {
            var declarations = new List<AstNode<PascalTokenType>>();

            while (CurrentToken.Type == PascalTokenType.KwVar)
            {
                Eat(PascalTokenType.KwVar);

                // Out of loop, grammar requires 1 run.
                declarations.Add(VariableDeclaration());
                Eat(PascalTokenType.SemCol);

                while (CurrentToken.Type == PascalTokenType.Id)
                {
                    declarations.Add(VariableDeclaration());

                    Eat(PascalTokenType.SemCol);
                }
            }

            while (CurrentToken.Type == PascalTokenType.KwProcedure ||
                   CurrentToken.Type == PascalTokenType.KwFunction)
            {
                if (CurrentToken.Type == PascalTokenType.KwProcedure)
                {
                    // TODO: take looping logic out of function, move to here.
                    declarations.AddRange(ProcedureDeclarations());
                }
                else
                {
                    declarations.Add(FunctionDeclaration());
                }
            }

            return declarations;
        }

        private List<ProcedureDeclarationNode<PascalTokenType>> ProcedureDeclarations()
        {
            var declarations = new List<ProcedureDeclarationNode<PascalTokenType>>();

            while (CurrentToken.Type == PascalTokenType.KwProcedure)
            {
                var procNode = new ProcedureDeclarationNode<PascalTokenType>();

                Eat(PascalTokenType.KwProcedure);

                procNode.Name = Variable().Name;

                // If parameters are found, eat them.
                // TODO.
                if (CurrentToken.Type == PascalTokenType.ParL)
                {
                    procNode.Parameters = new List<VarDeclNode<PascalTokenType>>();

                    Eat(PascalTokenType.ParL);

                    procNode.Parameters.Add(VariableDeclaration());

                    while (CurrentToken.Type == PascalTokenType.SemCol)
                    {
                        Eat(PascalTokenType.SemCol);

                        procNode.Parameters.Add(VariableDeclaration());
                    }

                    Eat(PascalTokenType.ParR);
                }

                Eat(PascalTokenType.SemCol);

                procNode.Block = Block();

                Eat(PascalTokenType.SemCol);

                declarations.Add(procNode);
            }

            return declarations;
        }

        /// <summary>
        /// function-declaration : FUNCTION variable (PARL variable-declaration (SEMCOL variable-declaration)* PARR)? COLON type-spec SEMCOL block SEMCOL
        /// </summary>
        /// <returns></returns>
        private FunctionDeclarationNode<PascalTokenType> FunctionDeclaration()
        {
            var functionDeclaration = new FunctionDeclarationNode<PascalTokenType>();

            Eat(PascalTokenType.KwFunction);

            functionDeclaration.Name = Variable().Name;

            // If parameters are found, eat them.
            if (CurrentToken.Type == PascalTokenType.ParL)
            {
                functionDeclaration.Parameters = new List<VarDeclNode<PascalTokenType>>();

                Eat(PascalTokenType.ParL);

                functionDeclaration.Parameters.Add(VariableDeclaration());

                while (CurrentToken.Type == PascalTokenType.SemCol)
                {
                    Eat(PascalTokenType.SemCol);

                    functionDeclaration.Parameters.Add(VariableDeclaration());
                }

                Eat(PascalTokenType.ParR);
            }

            Eat(PascalTokenType.Colon);

            functionDeclaration.Type = TypeSpec();

            Eat(PascalTokenType.SemCol);

            functionDeclaration.Block = Block();

            Eat(PascalTokenType.SemCol);

            return functionDeclaration;
        }

        private VarDeclNode<PascalTokenType> VariableDeclaration()
        {
            var vars = new List<string>();
            var token = CurrentToken;

            vars.Add(CurrentToken.ValueAsString);
            Eat(PascalTokenType.Id);

            while (CurrentToken.Type == PascalTokenType.Comma)
            {
                Eat(PascalTokenType.Comma);

                vars.Add(CurrentToken.ValueAsString);
                Eat(PascalTokenType.Id);
            }

            Eat(PascalTokenType.Colon);

            return new VarDeclNode<PascalTokenType>
            {
                Variable = vars,
                Type = TypeSpec(),
                Token = token
            };
        }

        private TypeNode<PascalTokenType> TypeSpec()
        {
            TypeNode<PascalTokenType> token;

            switch (CurrentToken.Type)
            {
                case PascalTokenType.TypeInteger:
                    token = new TypeNode<PascalTokenType>
                    {
                        Token = CurrentToken,
                        Type = "INTEGER"
                    };
                    Eat(PascalTokenType.TypeInteger);
                    break;
                case PascalTokenType.TypeReal:
                    token = new TypeNode<PascalTokenType>
                    {
                        Token = CurrentToken,
                        Type = "REAL"
                    };
                    Eat(PascalTokenType.TypeReal);
                    break;
                case PascalTokenType.TypeString:
                    token = new TypeNode<PascalTokenType>
                    {
                        Token = CurrentToken,
                        Type = "STRING"
                    };
                    Eat(PascalTokenType.TypeString);
                    break;
                case PascalTokenType.TypeBoolean:
                    token = new TypeNode<PascalTokenType>
                    {
                        Token = CurrentToken,
                        Type = "BOOLEAN"
                    };
                    Eat(PascalTokenType.TypeBoolean);
                    break;
                case PascalTokenType.KwArray:
                    var arraytoken = new ArrayTypeNode<PascalTokenType>
                    {
                        Token = CurrentToken,
                        Type = "ARRAY"
                    };
                    Eat(PascalTokenType.KwArray);
                    Eat(PascalTokenType.Brl);
                    arraytoken.Subscript = TypeSpec();
                    Eat(PascalTokenType.Brr);
                    Eat(PascalTokenType.KwOf);
                    arraytoken.ArrayType = TypeSpec();
                    token = arraytoken;
                    break;
                case PascalTokenType.ConstInteger:
                    var anonType = new AnonymousTypeNode<PascalTokenType>
                    {
                        Token = CurrentToken,
                        Type = "SUBRANGE"
                    };
                    var rangeStart = CurrentToken.ValueAsInt;
                    Eat(PascalTokenType.ConstInteger);
                    Eat(PascalTokenType.DoubleDot);
                    var rangeEnd = CurrentToken.ValueAsInt;
                    Eat(PascalTokenType.ConstInteger);
                    anonType.Symbol = new SubrangeScalarDataTypeSymbol(rangeStart, rangeEnd);
                    token = anonType;
                    break;
                default:
                    throw new InvalidOperationException($"Token Type '{CurrentToken.Type}' is not a Type!");
            }

            return token;
        }

        private CompoundNode<PascalTokenType> CompoundStatement()
        {
            Eat(PascalTokenType.KwBegin);
            var stm = StatementList();
            Eat(PascalTokenType.KwEnd);

            return stm;
        }

        private CompoundNode<PascalTokenType> StatementList()
        {
            var statements = new List<AstNode<PascalTokenType>>
            {
                Statement()
            };

            while (CurrentToken.Type == PascalTokenType.SemCol)
            {
                Eat(PascalTokenType.SemCol);
                statements.Add(Statement());
            }

            if (CurrentToken.Type == PascalTokenType.Id)
                throw new InvalidOperationException();

            return new CompoundNode<PascalTokenType>
            {
                Children = statements
            };
        }

        private AstNode<PascalTokenType> Statement()
        {
            switch (CurrentToken.Type)
            {
                //: compound-statement
                case PascalTokenType.KwBegin:
                    return CompoundStatement();
                //| assignment-statement
                //| procedure-call
                case PascalTokenType.Id:
                    var varNode = Variable();
                    //| procedure-call
                    if (CurrentToken.Type == PascalTokenType.ParL)
                        return ProcedureCall(varNode);

                    // TODO split between procedure and assignment.
                    return AssignmentStatement(varNode);
                // | if-statement
                case PascalTokenType.KwIf:
                    return IfStatement();
                case PascalTokenType.KwWhile:
                    return WhileLoop();
                //| empty
                default:
                    return Empty();
            }
        }

        private ProcedureCallNode<PascalTokenType> ProcedureCall(VarNode<PascalTokenType> varNode)
        {
            var parameters = new List<AstNode<PascalTokenType>>();

            Eat(PascalTokenType.ParL);

            // Eat parameters.
            if (CurrentToken.Type != PascalTokenType.ParR)
            {
                parameters.Add(Expr());

                while (CurrentToken.Type == PascalTokenType.Comma)
                {
                    Eat(PascalTokenType.Comma);

                    parameters.Add(Expr());
                }
            }

            Eat(PascalTokenType.ParR);

            return new ProcedureCallNode<PascalTokenType>
            {
                ProcedureName = varNode.Name,
                Token = varNode.Token,
                Parameters = parameters
            };
        }

        private AstNode<PascalTokenType> AssignmentStatement(VarNode<PascalTokenType> varNode)
        {
            var token = CurrentToken;
            Eat(PascalTokenType.OpAssign);

            return new AssignNode<PascalTokenType>
            {
                Token = token,
                Variable = varNode,
                Expr = Expr()
            };
        }

        private VarNode<PascalTokenType> Variable()
        {
            var token = CurrentToken;
            Eat(PascalTokenType.Id);

            return Variable(token);
        }

        private VarNode<PascalTokenType> Variable(GenericToken<PascalTokenType> idToken)
        {
            return new VarNode<PascalTokenType>
            {
                Token = idToken,
                Name = (string) idToken.Value,
                // TODO: Type Unknown during parsing.
            };
        }

        private AstNode<PascalTokenType> Empty()
        {
            return new NoOpNode<PascalTokenType>();
        }

        private AstNodeValue<PascalTokenType> AddSub()
        {
            var term = Term();

            while (
                CurrentToken.Type == PascalTokenType.OpAdd ||
                CurrentToken.Type == PascalTokenType.OpSub
            )
            {
                var token = CurrentToken;

                Eat(PascalTokenType.OpAdd, PascalTokenType.OpSub);

                term = new BinOpNode<PascalTokenType>
                {
                    Left = term,
                    Token = token,
                    Right = Term(),
                    Type = term.Type //TODO: not 100% sure, might be different.
                };
            }

            return term;
        }

        private AstNodeValue<PascalTokenType> Term()
        {
            var factor = Factor();

            while (
                CurrentToken.Type == PascalTokenType.OpMul ||
                CurrentToken.Type == PascalTokenType.OpDiv ||
                CurrentToken.Type == PascalTokenType.OpIntDiv
            )
            {
                var token = CurrentToken;

                Eat(PascalTokenType.OpMul, PascalTokenType.OpDiv, PascalTokenType.OpIntDiv);

                factor = new BinOpNode<PascalTokenType>
                {
                    Left = factor,
                    Token = token,
                    Right = Factor(),
                    Type = factor.Type //TODO: not 100% sure, might be different.
                };
            }

            return factor;
        }

        private AstNodeValue<PascalTokenType> Factor()
        {
            var token = CurrentToken;
            switch (CurrentToken.Type)
            {
                //: ADD factor
                case PascalTokenType.OpAdd:
                    Eat(PascalTokenType.OpAdd);
                    var factorAdd = Factor();
                    return new UnaryOpNode<PascalTokenType>
                    {
                        Token = token,
                        Factor = factorAdd,
                        Type = factorAdd.Type
                    };
                //| SUB factor
                case PascalTokenType.OpSub:
                    Eat(PascalTokenType.OpSub);
                    var factorSub = Factor();
                    return new UnaryOpNode<PascalTokenType>
                    {
                        Token = token,
                        Factor = factorSub,
                        Type = factorSub.Type
                    };
                //| INTEGER
                case PascalTokenType.ConstInteger:
                    Eat(PascalTokenType.ConstInteger);
                    return new LiteralNode<PascalTokenType>
                    {
                        Token = token,
                        Value = token.ValueAsInt,
                        Type = "INTEGER"
                    };
                //| REAL
                case PascalTokenType.ConstReal:
                    Eat(PascalTokenType.ConstReal);
                    return new LiteralNode<PascalTokenType>
                    {
                        Token = token,
                        Value = token.ValueAsReal,
                        Type = "REAL"
                    };
                //| PARL expr PARR
                case PascalTokenType.ParL:
                    Eat(PascalTokenType.ParL);
                    var expr = Expr();
                    Eat(PascalTokenType.ParR);
                    return expr;
                case PascalTokenType.ConstString:
                case PascalTokenType.HashTag:
                    return StringLiteral(token);
                case PascalTokenType.KwTrue:
                    Eat(PascalTokenType.KwTrue);
                    return new LiteralNode<PascalTokenType>
                    {
                        Token = token,
                        Value = token.ValueAsBoolean,
                        Type = "BOOLEAN"
                    };
                case PascalTokenType.KwFalse:
                    Eat(PascalTokenType.KwFalse);
                    return new LiteralNode<PascalTokenType>
                    {
                        Token = token,
                        Value = token.ValueAsBoolean,
                        Type = "BOOLEAN"
                    };
                //| variable
                default:
                    var idToken = CurrentToken;
                    Eat(PascalTokenType.Id);
                    if (CurrentToken.Type == PascalTokenType.ParL)
                    {
                        return FunctionCall(idToken);
                    }

                    if (CurrentToken.Type == PascalTokenType.Brl)
                    {
                        return Index(idToken);
                    }

                    return Variable(idToken);
            }
        }

        private AstNodeValue<PascalTokenType> FunctionCall(GenericToken<PascalTokenType> idToken)
        {
            var varNode = Variable(idToken);
            var parameters = new List<AstNode<PascalTokenType>>();

            Eat(PascalTokenType.ParL);

            // Eat parameters.
            if (CurrentToken.Type != PascalTokenType.ParR)
            {
                parameters.Add(Expr());

                while (CurrentToken.Type == PascalTokenType.Comma)
                {
                    Eat(PascalTokenType.Comma);

                    parameters.Add(Expr());
                }
            }

            Eat(PascalTokenType.ParR);

            return new FunctionCallNode<PascalTokenType>
            {
                FunctionName = varNode.Name,
                Token = varNode.Token,
                Parameters = parameters
                // TODO: Make sure type is set on runtime/semantic analysis.
            };
        }

        private IfThenNode<PascalTokenType> IfStatement()
        {
            var node = new IfThenNode<PascalTokenType>
            {
                Token = CurrentToken
            };

            Eat(PascalTokenType.KwIf);
            Eat(PascalTokenType.ParL);

            node.Condition = Expr();

            Eat(PascalTokenType.ParR);

            Eat(PascalTokenType.KwThen);

            node.Then = Statement();

            if (CurrentToken.Type == PascalTokenType.KwElse)
            {
                Eat(PascalTokenType.KwElse);

                node.Else = Statement();
            }

            return node;
        }

        private AstNodeValue<PascalTokenType> Expr()
        {
            var factor = AddSub();

            while (
                CurrentToken.Type == PascalTokenType.Equals ||
                CurrentToken.Type == PascalTokenType.LessThan ||
                CurrentToken.Type == PascalTokenType.GreaterThan
            )
            {
                var token = CurrentToken;

                Eat(PascalTokenType.Equals,
                    PascalTokenType.GreaterThan,
                    PascalTokenType.LessThan);

                factor = new BinOpNode<PascalTokenType>
                {
                    Left = factor,
                    Token = token,
                    Right = AddSub(),
                    Type = factor.Type // TODO: Type not 100% sure, might be different
                };
            }

            return factor;
        }

        private IndexNode<PascalTokenType> Index(GenericToken<PascalTokenType> idToken)
        {
            var node = new IndexNode<PascalTokenType>
            {
                Token = idToken,
                Variable = Variable(idToken)
            };

            Eat(PascalTokenType.Brl);
            node.Expr = Expr();
            Eat(PascalTokenType.Brr);

            return node;
        }

        private WhileDoNode<PascalTokenType> WhileLoop()
        {
            var node = new WhileDoNode<PascalTokenType>
            {
                Token = CurrentToken,
            };

            Eat(PascalTokenType.KwWhile);

            node.Condition = Expr();

            Eat(PascalTokenType.KwDo);

            node.Statement = Statement();

            return node;
        }

        private LiteralNode<PascalTokenType> StringLiteral(GenericToken<PascalTokenType> token)
        {
            if (token.Type == PascalTokenType.ConstString)
            {
                Eat(PascalTokenType.ConstString);
                return new LiteralNode<PascalTokenType>
                {
                    Token = token,
                    Value = token.ValueAsString,
                    Type = "STRING"
                };
            }

            if (token.Type == PascalTokenType.HashTag)
            {
                Eat(PascalTokenType.HashTag);
                var intToken = CurrentToken;
                Eat(PascalTokenType.ConstInteger);
                return new LiteralNode<PascalTokenType>
                {
                    Token = token,
                    Value = ((char)intToken.ValueAsInt).ToString(),
                    Type = "STRING"
                };
            }

            // TODO: can't read a proper string literal.
            throw new InvalidOperationException();
        }
    }
}