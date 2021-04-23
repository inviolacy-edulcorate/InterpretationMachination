using System;
using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.Tokens
{
    public class TokenSet<T> where T : Enum
    {
        public TokenSet()
        {
            TokenToTypeMap = new Dictionary<string, T>();
            TypeToTokenMap = new Dictionary<T, string>();
            IntegerTypes = new List<T>();
            RealTypes = new List<T>();
            StringTypes = new List<T>();
            BooleanTypes = new List<T>();

            WhitespaceCharacters = new HashSet<string>();
            NewLineCharacters = new HashSet<string>();
            CommentStartEndCharacters = new Dictionary<string, string>();
        }

        public Dictionary<T, string> TypeToTokenMap { get; }
        public Dictionary<string, T> TokenToTypeMap { get; }

        public HashSet<string> WhitespaceCharacters { get; }
        public HashSet<string> NewLineCharacters { get; }
        public Dictionary<string, string> CommentStartEndCharacters { get; }

        public string StringStartCharacter { get; set; }
        //public string StringEscapeCharacter { get; set; }

        /// <summary>
        /// The first character pattern to match to start the Numeric reading.
        /// </summary>
        public string NumericRecognizePattern { get; set; }
        /// <summary>
        /// The pattern to continue reading a number.
        /// </summary>
        public string NumericPattern { get; set; }
        /// <summary>
        /// The first character pattern to match to start the Id reading.
        /// </summary>
        public string IdRecognizePattern { get; set; }
        /// <summary>
        /// The pattern to continue reading an Id.
        /// </summary>
        public string IdPattern { get; set; }

        public T EndOfStreamTokenType { get; set; }
        public T DoubleTokenType { get; set; }
        public T IntegerTokenType { get; set; }
        public T StringTokenType { get; set; }
        public T IdTokenType { get; set; }

        public List<T> IntegerTypes { get; }
        public List<T> RealTypes { get; }
        public List<T> StringTypes { get; }
        public List<T> BooleanTypes { get; }

        public string this[T index]
        {
            get => TypeToTokenMap.ContainsKey(index) ? TypeToTokenMap[index] : default;
            set
            {
                TokenToTypeMap[value] = index;
                TypeToTokenMap[index] = value;
            }
        }

        public T this[string index]
        {
            get => TokenToTypeMap.ContainsKey(index) ? TokenToTypeMap[index] : default;
            set
            {
                TokenToTypeMap[index] = value;
                TypeToTokenMap[value] = index;
            }
        }

        public bool IsIntegerType(T type)
        {
            return IntegerTypes.Contains(type);
        }

        public bool IsRealType(T type)
        {
            return RealTypes.Contains(type);
        }

        public bool IsStringType(T type)
        {
            return StringTypes.Contains(type);
        }

        public bool IsBooleanType(T type)
        {
            return BooleanTypes.Contains(type);
        }

        
    }
}