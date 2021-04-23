using System;
using InterpretationMachination.DataStructures.AbstractSyntaxTree;
using InterpretationMachination.PascalInterpreter.AstNodes;

namespace InterpretationMachination.PascalInterpreter
{
    /// <summary>
    /// Base class for Abstract Syntax Tree Visitors.
    /// </summary>
    /// <typeparam name="T">TokenSet to use.</typeparam>
    public abstract class AstVisitor<T> where T : Enum
    {
        public void VisitTree(AstNode<T> rootNode)
        {
            VisitAstNode(rootNode);
        }

        protected object VisitAstNode(AstNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            switch (node)
            {
                case BinOpNode<T> binOpNode:
                    return VisitBinOpNode(binOpNode);
                    break;

                case LiteralNode<T> numNode:
                    return VisitNumericNode(numNode);

                case UnaryOpNode<T> unaryOpNode:
                    return VisitUnaryOpNode(unaryOpNode);
                    break;

                case CompoundNode<T> compoundNode:
                    VisitCompoundNode(compoundNode);
                    break;

                case AssignNode<T> assignNode:
                    VisitAssignNode(assignNode);
                    break;

                case VarNode<T> varNode:
                    return VisitVariableNode(varNode);
                    break;

                case NoOpNode<T> noOpNode:
                    VisitNoOpNode(noOpNode);
                    break;

                case ProgramNode<T> programNode:
                    VisitProgramNode(programNode);
                    break;

                case BlockNode<T> blockNode:
                    VisitBlockNode(blockNode);
                    break;

                case VarDeclNode<T> varDeclNode:
                    VisitVarDeclarationNode(varDeclNode);
                    break;

                case TypeNode<T> typeNode:
                    VisitTypeNode(typeNode);
                    break;

                case ProcedureDeclarationNode<T> procedureNode:
                    VisitProcedureNode(procedureNode);
                    break;

                case ProcedureCallNode<T> procedureCallNode:
                    VisitProcedureCallNode(procedureCallNode);
                    break;

                case FunctionDeclarationNode<T> functionDeclarationNode:
                    VisitFunctionDeclarationNode(functionDeclarationNode);
                    break;

                case FunctionCallNode<T> functionCallNode:
                    return VisitFunctionCallNode(functionCallNode);
                    break;

                case IfThenNode<T> ifThenNode:
                    VisitIfThenNode(ifThenNode);
                    break;

                case IndexNode<T> indexNode:
                    return VisitIndexNode(indexNode);
                    break;

                case WhileDoNode<T> whileDoNode:
                    VisitWhileDoNode(whileDoNode);
                    break;

                default:
                    throw new UnknownNodeTypeException<T>(node);
            }

            return null;
        }

        protected virtual void VisitWhileDoNode(WhileDoNode<T> whileDoNode)
        {
            VisitAstNode(whileDoNode.Condition);
            VisitAstNode(whileDoNode.Statement);
        }

        protected virtual object VisitIndexNode(IndexNode<T> indexNode)
        {
            VisitAstNode(indexNode.Variable);
            VisitAstNode(indexNode.Expr);

            return null;
        }

        protected virtual void VisitProcedureCallNode(ProcedureCallNode<T> procedureCallNode)
        {
            foreach (var parameter in procedureCallNode.Parameters)
            {
                VisitAstNode(parameter);
            }
        }

        protected virtual void VisitProcedureNode(ProcedureDeclarationNode<T> procedureDeclarationNode)
        {
            if (procedureDeclarationNode.Parameters != null)
            {
                foreach (var procedureNodeParameter in procedureDeclarationNode.Parameters)
                {
                    VisitAstNode(procedureNodeParameter);
                }
            }

            VisitAstNode(procedureDeclarationNode.Block);
        }

        protected virtual void VisitProgramNode(ProgramNode<T> programNode)
        {
            VisitAstNode(programNode.Block);
        }

        protected virtual object VisitUnaryOpNode(UnaryOpNode<T> unaryOpNode)
        {
            VisitAstNode(unaryOpNode.Factor);

            return null;
        }

        protected virtual void VisitCompoundNode(CompoundNode<T> compoundNode)
        {
            foreach (var compoundNodeChild in compoundNode.Children)
            {
                VisitAstNode(compoundNodeChild);
            }
        }

        protected virtual void VisitAssignNode(AssignNode<T> assignNode)
        {
            VisitAstNode(assignNode.Variable);
            VisitAstNode(assignNode.Expr);
        }

        protected virtual object VisitVariableNode(VarNode<T> varNode)
        {
            return null;
        }

        protected virtual void VisitNoOpNode(NoOpNode<T> noOpNode)
        {
        }

        protected virtual void VisitBlockNode(BlockNode<T> blockNode)
        {
            foreach (var blockNodeDeclaration in blockNode.Declarations)
            {
                VisitAstNode(blockNodeDeclaration);
            }

            VisitAstNode(blockNode.CompoundStatement);
        }

        protected virtual void VisitVarDeclarationNode(VarDeclNode<T> varDeclNode)
        {
            VisitAstNode(varDeclNode.Type);
        }

        protected virtual void VisitTypeNode(TypeNode<T> typeNode)
        {
        }

        protected virtual object VisitBinOpNode(BinOpNode<T> node)
        {
            VisitAstNode(node.Left);
            VisitAstNode(node.Right);

            return null;
        }

        protected virtual object VisitNumericNode(LiteralNode<T> node)
        {
            return null;
        }

        protected virtual object VisitFunctionCallNode(FunctionCallNode<T> node)
        {
            if (node.Parameters != null)
            {
                foreach (var procedureNodeParameter in node.Parameters)
                {
                    VisitAstNode(procedureNodeParameter);
                }
            }

            return null;
        }

        protected virtual void VisitFunctionDeclarationNode(FunctionDeclarationNode<T> node)
        {
            if (node.Parameters != null)
            {
                foreach (var procedureNodeParameter in node.Parameters)
                {
                    VisitAstNode(procedureNodeParameter);
                }
            }

            VisitAstNode(node.Type);

            VisitAstNode(node.Block);
        }

        protected virtual void VisitIfThenNode(IfThenNode<T> ifThenNode)
        {
            VisitAstNode(ifThenNode.Condition);
            VisitAstNode(ifThenNode.Then);
            if (ifThenNode.Else != null)
            {
                VisitAstNode(ifThenNode.Else);
            }
        }
    }

    public class UnknownNodeTypeException<T> : Exception where T : Enum
    {
        private const string MessageFormat = "AstNode type '{0}' is not supported.";

        public UnknownNodeTypeException(AstNode<T> node)
            : base(string.Format(MessageFormat, node.GetType().Namespace + "." + node.GetType().Name))
        {
        }
    }
}