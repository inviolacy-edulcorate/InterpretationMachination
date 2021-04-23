namespace InterpretationMachination.Calculator.AstNodes
{
    public class UnaryOpNode : AstNode
    {
        public AstNode Factor { get; set; }
    }
}