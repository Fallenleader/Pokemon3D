using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    /// <summary>
    /// Different error types for hard coded error messages.
    /// </summary>
    internal enum ErrorType
    {
        SyntaxError,
        TypeError,
        ReferenceError,
        APIError
    }

    /// <summary>
    /// Handles <see cref="ScriptProcessor"/> errors.
    /// </summary>
    internal class ErrorHandler
    {
        #region Error Messages

        public const string MESSAGE_TYPE_NOT_A_FUNCTION = "{0} is not a function";

        public const string MESSAGE_REFERENCE_NOT_DEFINED = "{0} is not defined";

        public const string MESSAGE_SYNTAX_INVALID_INCREMENT = "invalid increment operand";
        public const string MESSAGE_SYNTAX_INVALID_DECREMENT = "invalid decrement operand";
        public const string MESSAGE_SYNTAX_MISSING_FORMAL_PARAMETER = "missing formal parameter";
        public const string MESSAGE_SYNTAX_MISSING_FUNCTION_BODY = "missing function body";

        public const string MESSAGE_API_NOT_SUPPORTED = "this functionality is not supported";

        #endregion

        private ScriptProcessor _processor;
        private SObject _errorObject;

        public bool ThrownError
        {
            get { return _errorObject != null; }
        }

        public SObject ErrorObject
        {
            get { return _errorObject; }
            private set
            {
                if (_errorObject == null)
                    _errorObject = value;
            }
        }

        public ErrorHandler(ScriptProcessor processor)
        {
            _processor = processor;
        }

        public SObject ThrowError(SObject errorObject)
        {
            ErrorObject = errorObject;
            return ErrorObject;
        }

        public SObject ThrowError(ErrorType errorType, string message)
        {
            string strErrorType = errorType.ToString();

            //TODO: Create error object here and put it as argument in the method call.

            return ThrowError(null);
        }

        public SObject ThrowError(ErrorType errorType, string message, object[] messageArgs)
        {
            return ThrowError(errorType, string.Format(message, messageArgs));
        }
    }
}
