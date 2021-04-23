using System;
using System.Collections.Generic;
using System.IO;
using InterpretationMachination.DataStructures.Tokens;

namespace IM.Lexers.Interfaces
{
    /// <summary>
    /// Defines the public interface for a Lexer producing <see cref="GenericToken{T}"/>.
    /// </summary>
    /// <typeparam name="T">Token type to use. Has to be Enum.</typeparam>
    public interface ILexer<T> : IEnumerable<GenericToken<T>> where T : Enum
    {
        public TokenSet<T> TokenSet { get; }

        public IDictionary<string, T> KeyWords { get; }

        GenericToken<T> GetNextToken();
    }
}