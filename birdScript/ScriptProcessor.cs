using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    public class ScriptProcessor
    {
        #region Public interface

        public ScriptProcessor()
        {
            ErrorHandler = new ErrorHandler(this);
        }

        public ScriptProcessor(ScriptContext context)
        {
            
        }

        public SObject Run(string code)
        {
            return null;
        }

        #endregion

        internal static readonly string[] ReservedKeywords = new string[] { "if", "else", "while", "for", "function", "class", "constructor", "using", "var", "static", "new", "extends", "this", "super", "link", "readonly", "break", "continue", "indexer", "get", "set", "throw", "try", "catch", "finally" };

        /// <summary>
        /// Returns if the given string is a valid identifier.
        /// </summary>
        /// <param name="identifier">The string to check.</param>
        internal static bool IsValidIdentifier(string identifier)
        {
            // The string must not be empty string, and start with a unicode letter.
            // Also, it cannot be a reserved keyword.
            return !(string.IsNullOrEmpty(identifier) || 
                !char.IsLetter(identifier[0]) || 
                ReservedKeywords.Contains(identifier));
        }

        internal ErrorHandler ErrorHandler { get; }
        internal ScriptContext Context { get; }

        /// <summary>
        /// The undefined object.
        /// </summary>
        internal SObject Undefined
        {
            get { return null; }
        }

        /// <summary>
        /// The null "object".
        /// </summary>
        internal SObject Null
        {
            get { return null; }
        }

        /// <summary>
        /// Creates an instance of the string primitive.
        /// </summary>
        internal SString CreateString(string value)
        {
            return null;
        }

        /// <summary>
        /// Creates an instance of the string primitive, also setting the escaped status.
        /// </summary>
        internal SString CreateString(string value, bool escaped)
        {
            return null;
        }

        /// <summary>
        /// Creates an instance of the number primitive.
        /// </summary>
        internal SNumber CreateNumber(double value)
        {
            return null;
        }

        /// <summary>
        /// Creates an instance of the bool primitive.
        /// </summary>
        internal SBool CreateBool(bool value)
        {
            return null;
        }
    }
}
