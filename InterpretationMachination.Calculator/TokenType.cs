namespace InterpretationMachination.DataStructures.Tokens
{
    public enum TokenType
    {
        None,
        EndOfFile,
        Whitespace,

        Integer,

        OpAdd,
        OpSub,
        OpDiv,
        OpMul,
        OpAssign,

        KwBegin,
        KwEnd,

        Id,

        SemCol,
        Dot,
        ParL,
        ParR,
    }
}