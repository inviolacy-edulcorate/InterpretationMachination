namespace InterpretationMachination.Calculator.AstNodes
{
    public class BinOpNode : AstNode
    {
        public AstNode Left { get; set; }
        public AstNode Right { get; set; }
    }
}