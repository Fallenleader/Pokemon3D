using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    internal enum ErrorType
    {
        SyntaxError,
        TypeError,
        ReferenceError
    }

    internal class ErrorHandler
    {
        #region Error Messages

        public const string MESSAGE_REFERENCE_NOT_DEFINED = "{0} is not defined";

        public const string MESSAGE_SYNTAX_INVALID_INCREMENT = "invalid increment operand";
        public const string MESSAGE_SYNTAX_INVALID_DECREMENT = "invalid decrement operand";

        #endregion

        private ScriptProcessor _processor;

        public bool ThrownError { get; private set; }

        public SObject ErrorObject { get; private set; }

        public ErrorHandler(ScriptProcessor processor)
        {
            _processor = processor;
        }

        public SObject ThrowError(ErrorType errorType, string message)
        {
            string strErrorType = errorType.ToString();

            //TODO: Create error object here.

            ThrownError = true;
            return ErrorObject;
        }

        public SObject ThrowError(ErrorType errorType, string message, object[] messageArgs)
        {
            return ThrowError(errorType, string.Format(message, messageArgs));
        }
    }
}
