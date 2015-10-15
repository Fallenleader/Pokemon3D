using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    /// <summary>
    /// An internal exception class that gets thrown by the <see cref="ErrorHandler"/>.
    /// </summary>
    internal class ScriptException : Exception
    {
        /// <summary>
        /// The error object containing the error information.
        /// </summary>
        public SObject ErrorObject { get; }

        public ScriptException(SObject errorObject)
        {
            ErrorObject = errorObject;
        }
    }
}
