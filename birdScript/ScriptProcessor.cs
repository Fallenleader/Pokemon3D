using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using System.Text.RegularExpressions;

namespace birdScript
{
    /// <summary>
    /// A class to process birdScript scripts.
    /// </summary>
    public partial class ScriptProcessor
    {
        private struct ElementCapture
        {
            public int StartIndex;
            public int Length;
            public string Identifier;
            public int Depth;
        }

        private const string IDENTIFIER_SEPERATORS = "-+*/=!%&|<>,";

        /// <summary>
        /// The <see cref="birdScript.ErrorHandler"/> associated with this <see cref="ScriptProcessor"/>.
        /// </summary>
        internal ErrorHandler ErrorHandler { get; }
        /// <summary>
        /// The <see cref="ScriptContext"/> associated with this <see cref="ScriptProcessor"/>.
        /// </summary>
        internal ScriptContext Context { get; }

        private ScriptStatement[] _statements;
        private string _source;
        private bool _hasParent = false;

        private bool _returnIssued = false;
        private bool _continueIssued = false;
        private bool _breakIssued = false;

        #region Public interface

        public ScriptProcessor() : this(null) { }

        public ScriptProcessor(ScriptContext context)
        {
            _hasParent = true;

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
            _source = code;

            SObject returnObject = Undefined;

            if (_hasParent)
            {
                returnObject = ProcessStatements();
            }
            else
            {
                try
                {
                    returnObject = ProcessStatements();
                }
                catch (ScriptException ex)
                {
                    returnObject = ex.ErrorObject;
                }
            }

            return returnObject;
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

        #region Statement processing

        private SObject ProcessStatements()
        {
            SObject returnObject = Undefined;

            _statements = StatementProcessor.GetStatements(this, _source);

            int index = 0;

            while (index < _statements.Length)
            {
                returnObject = ExecuteStatement(_statements[index]);
                if (_continueIssued || _breakIssued || _returnIssued)
                {
                    returnObject = SObject.Unbox(returnObject);
                    return returnObject;
                }

                index++;
            }

            return SObject.Unbox(returnObject);
        }

        internal SObject ExecuteStatement(ScriptStatement statement)
        {
            switch (statement.StatementType)
            {
                case StatementType.Executable:
                    return ExecuteExecutable(statement);
                case StatementType.If:
                    break;
                case StatementType.Else:
                    break;
                case StatementType.ElseIf:
                    break;
                case StatementType.Using:
                    break;
                case StatementType.Var:
                    break;
                case StatementType.While:
                    break;
                case StatementType.Return:
                    break;
                case StatementType.Assignment:
                    break;
                case StatementType.For:
                    break;
                case StatementType.Function:
                    break;
                case StatementType.Class:
                    break;
                case StatementType.Link:
                    break;
                case StatementType.Continue:
                    break;
                case StatementType.Break:
                    break;
                case StatementType.Throw:
                    break;
                case StatementType.Try:
                    break;
                case StatementType.Catch:
                    break;
                case StatementType.Finally:
                    break;
                default:
                    break;
            }

            return null;
        }

        private SObject ExecuteExecutable(ScriptStatement statement)
        {
            if (statement.IsCompoundStatement)
            {
                ScriptProcessor processor = new ScriptProcessor(Context);

                // Remove { and }:
                string code = statement.Code.Remove(0, 1);
                code = code.Remove(code.Length - 1, 1);

                var returnObject = processor.Run(code);

                _breakIssued = processor._breakIssued;
                _continueIssued = processor._continueIssued;
                _returnIssued = processor._returnIssued;

                return returnObject;
            }
            else
            {

            }
            return null; //TO AVOID ERRORS FOR NOW
        }

        #endregion

        #region Helpers

        private string ResolveParentheses(string exp)
        {
            if (exp.Contains("(") && exp.Contains(")"))
            {
                int index = 0;
                int depth = 0;
                int parenthesesStartIndex = -1;
                StringBuilder newExpression = new StringBuilder();
                StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(exp, 0);

                while (index < exp.Length)
                {
                    char t = exp[index];
                    escaper.CheckStartAt(index);

                    if (!escaper.IsString)
                    {
                        if (t == '{' || t == '[')
                        {
                            depth++;
                            if (parenthesesStartIndex == -1)
                                newExpression.Append(t);
                        }
                        else if (t == '}' || t == ']')
                        {
                            depth--;
                            if (parenthesesStartIndex == -1)
                                newExpression.Append(t);
                        }
                        else if (t == '(')
                        {
                            if (depth == 0 && parenthesesStartIndex == -1)
                            {
                                ElementCapture capture = CaptureLeft(newExpression.ToString(), newExpression.Length - 1);
                                string identifier = capture.Identifier;
                                if (capture.Depth == 0 || identifier == "")
                                {
                                    if (identifier.Length == 0)
                                    {
                                        parenthesesStartIndex = index;
                                    }
                                    else
                                    {
                                        string testExp = newExpression.ToString().Remove(capture.StartIndex).Trim();

                                        if (testExp.Length > 0 && testExp.Last() == '.' || StatementProcessor.controlStatements.Contains(identifier) || identifier.StartsWith("new "))
                                        {
                                            newExpression.Append(t);
                                        }
                                        else
                                        {
                                            newExpression.Append(".call" + t);
                                        }
                                        depth++;
                                    }
                                }
                                else
                                {
                                    newExpression.Append(t);
                                    depth++;
                                }
                            }
                            else
                            {
                                depth++;
                                if (parenthesesStartIndex == -1)
                                    newExpression.Append(t);
                            }
                        }
                        else if (t == ')')
                        {
                            if (depth == 0 && parenthesesStartIndex > -1)
                            {
                                string parenthesesCode = exp.Substring(parenthesesStartIndex + 1, index - parenthesesStartIndex - 1);

                                if (parenthesesCode.Length > 0)
                                {
                                    if (parenthesesCode.Contains("=>") && Regex.IsMatch(parenthesesCode, REGEX_LAMBDA))
                                    {
                                        newExpression.Append(BuildLambdaFunction(parenthesesCode));
                                    }
                                    else
                                    {
                                        var returnObject = ExecuteStatement(new ScriptStatement(parenthesesCode));
                                        newExpression.Append(returnObject.ToScriptObject());
                                    }
                                }

                                parenthesesStartIndex = -1;
                            }
                            else
                            {
                                depth--;
                                if (parenthesesStartIndex == -1)
                                    newExpression.Append(t);
                            }
                        }
                        else
                        {
                            if (parenthesesStartIndex == -1)
                                newExpression.Append(t);
                        }
                    }
                    else
                    {
                        if (parenthesesStartIndex == -1)
                            newExpression.Append(t);
                    }

                    index++;
                }

                return newExpression.ToString();
            }

            return exp;
        }

        private string BuildLambdaFunction(string lambdaCode)
        {
            string signatureCode = lambdaCode.Remove(lambdaCode.IndexOf("=>")).Trim();
            return null; //TO AVOID ERRORS FOR NOW
        }

        /// <summary>
        /// Captures an element right from the starting index.
        /// </summary>
        private ElementCapture CaptureRight(string exp, int index)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return new ElementCapture() { Length = 0, StartIndex = 0, Identifier = "", Depth = 0 };

            string identifier = "";
            bool foundSeperatorChar = false;
            int depth = 0;
            StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(exp, index);

            while (index < exp.Length && !foundSeperatorChar)
            {
                char t = exp[index];
                escaper.CheckStartAt(index);

                if (!escaper.IsString)
                {
                    if (t == ')' || t == ']' || t == '}')
                    {
                        depth--;
                    }
                    else if (t == '(' || t == '[' || t == '{')
                    {
                        depth++;
                    }
                }
                if (t == '-' && string.IsNullOrWhiteSpace(identifier))
                {
                    identifier += "-";
                }
                else
                {
                    if (!escaper.IsString && depth == 0)
                    {
                        if (t == '.')
                        {
                            // Check if the '.' is not a decimal seperator:
                            if (!Regex.IsMatch(identifier.Trim(), REGEX_NUMLEFTDOT))
                            {
                                foundSeperatorChar = true;
                            }
                        }
                        else if (IDENTIFIER_SEPERATORS.Contains(t))
                        {
                            foundSeperatorChar = true;
                        }
                    }

                    // Append the char to the identifier:
                    if (!foundSeperatorChar)
                    {
                        identifier += t;
                    }
                }

                index++;
            }

            if (foundSeperatorChar)
                return new ElementCapture() { StartIndex = index - 1 - identifier.Length, Length = identifier.Length, Identifier = identifier.Trim(), Depth = depth };
            else
                return new ElementCapture() { StartIndex = index - identifier.Length, Length = identifier.Length, Identifier = identifier.Trim(), Depth = depth };
        }

        /// <summary>
        /// Captures an element left from the starting index.
        /// </summary>
        private ElementCapture CaptureLeft(string exp, int index)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return new ElementCapture() { Length = 0, StartIndex = 0, Identifier = "", Depth = 0 };

            string identifier = "";
            bool foundSeperatorChar = false;
            int depth = 0;
            StringEscapeHelper escaper = new RightToLeftStringEscapeHelper(exp, index);

            while (index >= 0 && !foundSeperatorChar)
            {
                char t = exp[index];
                escaper.CheckStartAt(index);

                if (!escaper.IsString)
                {
                    if (t == ')' || t == ']' || t == '}')
                    {
                        depth++;
                    }
                    else if (t == '(' || t == '[' || t == '{')
                    {
                        depth--;
                    }
                }

                if (depth < 0)
                {
                    // this is when we walk out of the capture area because we are inside some area and a )]} appeared.
                    foundSeperatorChar = true;
                }
                else
                {
                    if (!escaper.IsString && depth == 0)
                    {
                        if (t == '.')
                        {
                            // Check if the '.' is not a decimal seperator:
                            if (!Regex.IsMatch(identifier.Trim(), REGEX_NUMRIGHTDOT))
                            {
                                foundSeperatorChar = true;
                            }
                        }
                        else if (IDENTIFIER_SEPERATORS.Contains(t))
                        {
                            foundSeperatorChar = true;
                        }
                    }

                    // Append the char to the identifier:
                    if (!foundSeperatorChar)
                    {
                        identifier = t + identifier;
                    }
                }

                index--;
            }
            
            // Check for a minus in front of the identifier to indicate a negative number:
            if (index >= -1 && exp[index + 1] == '-' && !string.IsNullOrWhiteSpace(identifier))
            {
                identifier = "-" + identifier;
                index--;
            }

            if (foundSeperatorChar)
                return new ElementCapture() { StartIndex = index + 2, Length = identifier.Length, Identifier = identifier, Depth = depth };
            else
                return new ElementCapture() { StartIndex = index + 1, Length = identifier.Length, Identifier = identifier, Depth = depth };
        }

        #endregion
    }
}
