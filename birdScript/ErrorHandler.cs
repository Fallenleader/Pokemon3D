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
        public const string MESSAGE_TYPE_ABSTRACT_NO_EXTENDS = "an abstract class must extend Object.";

        public const string MESSAGE_REFERENCE_NOT_DEFINED = "{0} is not defined";
        public const string MESSAGE_REFERENCE_NO_PROTOTYPE = "{0} is not defined or not a prototype";

        public const string MESSAGE_SYNTAX_INVALID_INCREMENT = "invalid increment operand";
        public const string MESSAGE_SYNTAX_INVALID_DECREMENT = "invalid decrement operand";
        public const string MESSAGE_SYNTAX_MISSING_FORMAL_PARAMETER = "missing formal parameter";
        public const string MESSAGE_SYNTAX_MISSING_FUNCTION_BODY = "missing function body";
        public const string MESSAGE_SYNTAX_INVALID_CLASS_SIGNATURE = "invalid class signature";
        public const string MESSAGE_SYNTAX_MISSING_VAR_NAME = "missing variable name";
        public const string MESSAGE_SYNTAX_EXPECTED_EXPRESSION = "expected expression, got {0}";
        public const string MESSAGE_SYNTAX_UNTERMINATED_COMMENT = "unterminated comment";
        public const string MESSAGE_SYNTAX_MISSING_END_OF_COMPOUND_STATEMENT = "missing } in compound statement";

        public const string MESSAGE_SYNTAX_CLASS_EXTENDS_MISSING = "expected identifier after \"extends\" keyword";
        public const string MESSAGE_SYNTAX_CLASS_IDENTIFIER_MISSING = "expected class identifier";
        public const string MESSAGE_SYNTAX_CLASS_INVALID_STATEMENT = "only var and function statements are allowed inside class definitions";
        public const string MESSAGE_SYNTAX_CLASS_DUPLICATE_DEFINITION = "{0} is already declared in {1}.";
        public const string MESSAGE_SYNTAX_CLASS_INVALID_VAR_DECLARATION = "invalid variable declaration";
        public const string MESSAGE_SYNTAX_CLASS_FUNCTION_INDEXER_EXPECTED_TYPE = "function indexer expected indexer type";
        public const string MESSAGE_SYNTAX_CLASS_FUNCTION_INDEXER_INVALID_TYPE = "{0} is not a valid function indexer type";
        public const string MESSAGE_SYNTAX_CLASS_INVALID_FUNCTION_SIGNATURE = "invalid function signature";

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

        /// <summary>
        /// Throws an error with the given error object.
        /// </summary>
        public SObject ThrowError(SObject errorObject)
        {
            ErrorObject = errorObject;

            throw new ScriptException(ErrorObject);
        }

        /// <summary>
        /// Throws an error with the given <see cref="ErrorType"/> and error message.
        /// </summary>
        public SObject ThrowError(ErrorType errorType, string message)
        {
            string strErrorType = errorType.ToString();

            //TODO: Create error object here and put it as argument in the method call.
            
            return ThrowError(null);
        }

        /// <summary>
        /// Throws an error with the given <see cref="ErrorType"/> and message, adding formating options via message arguments.
        /// </summary>
        public SObject ThrowError(ErrorType errorType, string message, object[] messageArgs)
        {
            return ThrowError(errorType, string.Format(message, messageArgs));
        }
    }
}
