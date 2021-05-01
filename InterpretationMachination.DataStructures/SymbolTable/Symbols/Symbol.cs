namespace InterpretationMachination.DataStructures.SymbolTable.Symbols
{
    public class Symbol
    {
        public Symbol(string name, Symbol type)
        {
            Name = name;
            Type = type;
        }

        public Symbol(string name)
        {
            Name = name;
            Type = null;
        }

        public string Name { get; }
        public Symbol Type { get; }

        // TODO: Only for S2S Compiler.
        public int ScopeLevel { get; set; }

        public override string ToString()
        {
            return $"<{Name}:{Type}>";
        }
    }
}