using System;
using System.Collections.Generic;
using InterpretationMachination.DataStructures.SymbolTable.Symbols;

namespace InterpretationMachination.DataStructures.SymbolTable
{
    public class ScopedSymbolTable
    {
        public ScopedSymbolTable(string name, ScopedSymbolTable parentScope = null)
        {
            ParentScope = parentScope;
            Name = name;
            Table = new Dictionary<string, Symbol>();

            if (parentScope == null)
            {
                Depth = 0;
            }
            else
            {
                Depth = parentScope.Depth + 1;
            }
        }

        public ScopedSymbolTable() : this("BUILTINS")
        {
            Depth = 0;
        }

        private Dictionary<string, Symbol> Table { get; }

        public ScopedSymbolTable ParentScope { get; }

        public string Name { get; }

        public int Depth { get; }
        public IEnumerable<Symbol> DeclaredSymbols => Table.Values;

        public void DeclareSymbol(Symbol symbol)
        {
            if (Table.ContainsKey(symbol.Name.ToUpper()))
            {
                throw new InvalidOperationException($"[XXX] - Symbol '{symbol.Name}' has already been declared.");
            }

            Table[symbol.Name.ToUpper()] = symbol;

            symbol.ScopeLevel = Depth;
        }

        /// <summary>
        /// Lookup a symbol in this table or any of its parents.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol LookupSymbol(string name)
        {
            if (!Table.ContainsKey(name.ToUpper()))
            {
                if (ParentScope == null)
                {
                    return null;
                }
                else
                {
                    return ParentScope.LookupSymbol(name);
                }
            }

            return Table[name.ToUpper()];
        }

        /// <summary>
        /// Lookup a symbol in this table alone (non-recursive).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol LookupSymbolInThisTable(string name)
        {
            if (!Table.ContainsKey(name.ToUpper()))
            {
                return null;
            }

            return Table[name.ToUpper()];
        }
    }
}