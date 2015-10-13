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
            // The string must not be empty string, and start with a unicode letter:
            if (string.IsNullOrEmpty(identifier) || ReservedKeywords.Contains(identifier))
                return false;
            else
                return char.IsLetter(identifier[0]);
        }

        internal ErrorHandler ErrorHandler { get; }
        internal ScriptContext Context { get; }

        internal SObject Undefined
        {
            get { return null; }
        }

        internal SObject Null
        {
            get { return null; }
        }

        internal SString CreateString(string value)
        {
            return null;
        }

        internal SNumber CreateNumber(double value)
        {
            return null;
        }

        internal SBool CreateBool(bool value)
        {
            return null;
        }
    }
}
