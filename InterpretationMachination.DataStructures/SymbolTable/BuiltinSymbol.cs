namespace InterpretationMachination.DataStructures.SymbolTable
{
    public class BuiltinSymbol : Symbol
    {
        public BuiltinSymbol(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}