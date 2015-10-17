using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    /// <summary>
    /// A class to process birdScript scripts.
    /// </summary>
    public class ScriptProcessor
    {
        /// <summary>
        /// The <see cref="birdScript.ErrorHandler"/> associated with this <see cref="ScriptProcessor"/>.
        /// </summary>
        internal ErrorHandler ErrorHandler { get; }
        /// <summary>
        /// The <see cref="ScriptContext"/> associated with this <see cref="ScriptProcessor"/>.
        /// </summary>
        internal ScriptContext Context { get; }

        #region Public interface

        public ScriptProcessor() : this(null) { }

        public ScriptProcessor(ScriptContext context)
        {
            if (context != null && context.Parent == null)
                Context = context;
            else
                Context = new ScriptContext(this, context);

            Context.Initialize();

            ErrorHandler = new ErrorHandler(this);
        }

        /// <summary>
        /// Runs raw source code and returns the result.
        /// </summary>
        /// <param name="code">The source code to run.</param>
        /// <returns>Either data returned by a "return"-statement or the result of the last statement.</returns>
        public SObject Run(string code)
        {
            return null;
        }

        #endregion

        internal static readonly string[] ReservedKeywords = new string[] { "if", "else", "while", "for", "function", "class", "using", "var", "static", "new", "extends", "this", "super", "link", "readonly", "break", "continue", "indexer", "get", "set", "throw", "try", "catch", "finally" };

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

        /// <summary>
        /// The undefined object.
        /// </summary>
        internal SObject Undefined
        {
            get { return Context.GetVariable(SObject.LITERAL_UNDEFINED).Data; }
        }

        /// <summary>
        /// The null "object".
        /// </summary>
        internal SObject Null
        {
            get { return Context.GetVariable(SObject.LITERAL_NULL).Data; }
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
