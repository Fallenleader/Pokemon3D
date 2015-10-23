using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// Represents a function object that can be called in the script.
    /// </summary>
    internal class SFunction : SObject
    {
        private string[] _parameters;
        private DBuiltInMethod _method;

        /// <summary>
        /// The code body of the function.
        /// </summary>
        public string Body { get; set; }

        public SFunction(string body, string[] parameters)
        {
            Body = body;
            _parameters = parameters;
        }

        /// <summary>
        /// Initializes an instance with a script code signature and body.
        /// </summary>
        /// <param name="sourceCode">The source code, format: <code>function (params) { code }</code></param>
        public SFunction(ScriptProcessor processor, string sourceCode)
        {
            sourceCode = sourceCode.Trim();
            string paramCode = sourceCode.Remove(0, "function".Length).Trim().Remove(0, 1); //Removes "function", then any spaces between "function" and "(", then removes "(".
            paramCode = paramCode.Remove(paramCode.IndexOf(")"));

            _parameters = paramCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            bool allIdentifiersValid = true;
            int i = 0;

            while (i < _parameters.Length - 1 && allIdentifiersValid)
            {
                if (!ScriptProcessor.IsValidIdentifier(_parameters[i]))
                {
                    allIdentifiersValid = false;
                }

                i++;
            }

            if (!allIdentifiersValid)
            {
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_FORMAL_PARAMETER);
            }
            else
            {
                if (!sourceCode.Contains("{") || !sourceCode.EndsWith("}"))
                {
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_FUNCTION_BODY);
                }
                else
                {
                    Body = sourceCode.Remove(sourceCode.Length - 1, 1).Remove(0, sourceCode.IndexOf("{") + 1).Trim();
                }
            }
        }

        /// <summary>
        /// Initializes an instance with a built in method.
        /// </summary>
        public SFunction(DBuiltInMethod method)
        {
            _method = method;
        }

        internal override string TypeOf()
        {
            return LITERAL_TYPE_FUNCTION;
        }

        internal override double SizeOf()
        {
            if (_method != null)
                return 1;
            else
                return Body.Length;
        }

        private const string FUNCTION_SOURCE_FORMAT = "function({0}) {{ {1} }}";
        private const string FUNCTION_NATIVE_CODE_SOURCE = "[native code]";

        internal override string ToScriptSource()
        {
            string paramSource = "";
            if (_parameters != null)
            {
                foreach (string par in _parameters)
                {
                    if (paramSource.Length > 0)
                    {
                        paramSource += ", ";
                    }
                    paramSource += par;
                }
            }

            string bodySource = "";
            if (_method != null)
            {
                bodySource = FUNCTION_NATIVE_CODE_SOURCE;
            }
            else
            {
                bodySource = Body;
            }

            return string.Format(FUNCTION_SOURCE_FORMAT, paramSource, bodySource);
        }

        internal override string ToScriptObject()
        {
            return "$" + ObjectBuffer.GetObjectId(this).ToString();
        }

        internal override SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            return Call(processor, caller, This, parameters);
        }

        /// <summary>
        /// Executes the function.
        /// </summary>
        /// <param name="processor">The processor with context that called this functions.</param>
        /// <param name="caller">The calling object.</param>
        /// <param name="This">The "This" reference used in the call context.</param>
        /// <param name="parameters">The parameters used in this function call.</param>
        public SObject Call(ScriptProcessor processor, SObject caller, SObject This, SObject[] parameters)
        {
            ScriptProcessor functionProcessor = new ScriptProcessor(processor.Context);
            SObject functionReturnObject;

            if (_method != null)
            {
                functionReturnObject = _method(functionProcessor, caller, This, parameters);
            }
            else
            {
                for (int i = 0; i < _parameters.Length; i++)
                {
                    if (parameters.Length > i)
                    {
                        functionProcessor.Context.AddVariable(_parameters[i], parameters[i]);
                    }
                    else
                    {
                        functionProcessor.Context.AddVariable(_parameters[i], functionProcessor.Undefined);
                    }
                }

                functionProcessor.Context.This = This;
                functionReturnObject = functionProcessor.Run(Body);
            }
            
            return functionReturnObject;
        }
    }
}
