namespace InterpretationMachination.PascalInterpreter
{
    public enum PascalTokenType
    {
        None,
        EndOfFile,
        Whitespace,

        ConstInteger,
        ConstReal,
        ConstString,

        OpAdd,
        OpSub,
        OpDiv,
        OpIntDiv,
        OpMul,
        OpAssign,

        KwBegin,
        KwEnd,
        KwProgram,
        KwVar,
        KwProcedure,
        KwFunction,
        KwIf,
        KwThen,
        KwElse,
        KwFalse,
        KwTrue,

        TypeInteger,
        TypeReal,
        TypeString,
        TypeBoolean,

        Id,

        SemCol,
        Dot,
        ParL,
        ParR,
        Colon,
        Comma,
        Equals,
        GreaterThan,
        LessThan,
        Brl,
        Brr,
        KwWhile,
        KwDo,
        HashTag
    }
}