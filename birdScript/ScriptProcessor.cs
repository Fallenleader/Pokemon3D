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

        private const string IDENTIFIER_SEPARATORS = "-+*/=!%&|<>,";

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
                string exp = ResolveParentheses(statement.Code).Trim();

                #region QuickConvert

                // have quick conversions for small statements here
                // parameter statements are much faster that way:
                if (exp == SObject.LITERAL_BOOL_TRUE)
                {
                    return CreateBool(true);
                }
                else if (exp == SObject.LITERAL_BOOL_FALSE)
                {
                    return CreateBool(false);
                }
                else if (exp == SObject.LITERAL_UNDEFINED)
                {
                    return Undefined;
                }
                else if (exp == SObject.LITERAL_NULL)
                {
                    return Null;
                }
                else if (exp.StartsWith("\"") && exp.EndsWith("\"") && !exp.Remove(exp.Length - 1, 1).Remove(0, 1).Contains("\""))
                {
                    return CreateString(exp.Remove(exp.Length - 1, 1).Remove(0, 1));
                }

                #endregion

                if (exp.Contains("."))
                    exp = EvaluateOperatorLeftToRight(exp, ".");
                if (exp.Contains("++"))
                    exp = EvaluateOperatorLeftToRight(exp, "++");
                if (exp.Contains("--"))
                    exp = EvaluateOperatorLeftToRight(exp, "--");
                if (exp.Contains("**"))
                    exp = EvaluateOperatorLeftToRight(exp, "**");
                if (exp.Contains("*"))
                    exp = EvaluateOperatorLeftToRight(exp, "*");
                if (exp.Contains("/"))
                    exp = EvaluateOperatorLeftToRight(exp, "/");
                if (exp.Contains("%"))
                    exp = EvaluateOperatorLeftToRight(exp, "%");
                if (exp.Contains("+"))
                    exp = EvaluateOperatorLeftToRight(exp, "+");
                if (exp.Contains("-"))
                    exp = EvaluateOperatorLeftToRight(exp, "-");
                if (exp.Contains("<="))
                    exp = EvaluateOperatorLeftToRight(exp, "<=");
                if (exp.Contains(">="))
                    exp = EvaluateOperatorLeftToRight(exp, ">=");
                if (exp.Contains("<"))
                    exp = EvaluateOperatorLeftToRight(exp, "<");
                if (exp.Contains(">"))
                    exp = EvaluateOperatorLeftToRight(exp, ">");
                if (exp.Contains("==="))
                    exp = EvaluateOperatorLeftToRight(exp, "===");
                if (exp.Contains("!=="))
                    exp = EvaluateOperatorLeftToRight(exp, "!==");
                if (exp.Contains("=="))
                    exp = EvaluateOperatorLeftToRight(exp, "==");
                if (exp.Contains("!="))
                    exp = EvaluateOperatorLeftToRight(exp, "!=");
                if (exp.Contains("&&"))
                    exp = EvaluateOperatorLeftToRight(exp, "&&");
                if (exp.Contains("||"))
                    exp = EvaluateOperatorLeftToRight(exp, "||");

                return ToScriptObject(exp);
            }
        }

        #endregion

        #region Operators

        private string EvaluateOperatorRightToLeft()
        {
            return null;
        }

        private string EvaluateOperatorLeftToRight(string exp, string op)
        {
            int[] ops = GetOperatorPositions(exp, op);

            for (int i = 0; i < ops.Length; i++)
            {
                var cOp = ops[i];

                bool needRight = true;

                if (op == "++" || op == "--")
                {
                    needRight = false;
                }

                ElementCapture captureLeft = CaptureLeft(exp, cOp - 1);
                string elementLeft = captureLeft.Identifier;

                if (!(op == "-" && elementLeft.Length == 0))
                {
                    string result = "";

                    SObject objectLeft = ToScriptObject(elementLeft);

                    ElementCapture captureRight;
                    string elementRight;
                    SObject objectRight = null;

                    if (needRight)
                    {
                        captureRight = CaptureRight(exp, cOp + op.Length);
                        elementRight = captureRight.Identifier;

                        if (op != ".")
                            objectRight = ToScriptObject(elementRight);
                    }
                    else
                    {
                        elementRight = "";
                        captureRight = new ElementCapture() { Length = 0 };
                    }

                    if (op != "." || IsDotOperatorDecimalSeparator(elementLeft, elementRight))
                    {
                        switch (op)
                        {
                            case ".":
                                //TODO: call member invoke.
                                break;
                            case "+":
                                result = ObjectOperators.AddOperator(this, objectLeft, objectRight);
                                break;
                            case "-":
                                result = ObjectOperators.SubtractOperator(this, objectLeft, objectRight);
                                break;
                            case "*":
                                result = ObjectOperators.MultiplyOperator(this, objectLeft, objectRight);
                                break;
                            case "/":
                                result = ObjectOperators.DivideOperator(this, objectLeft, objectRight);
                                break;
                            case "%":
                                result = ObjectOperators.ModulusOperator(this, objectLeft, objectRight);
                                break;
                            case "**":
                                result = ObjectOperators.ExponentOperator(this, objectLeft, objectRight);
                                break;
                            case "==":
                                result = ObjectOperators.EqualsOperator(this, objectLeft, objectRight);
                                break;
                            case "===":
                                result = ObjectOperators.TypeEqualsOperator(this, objectLeft, objectRight);
                                break;
                            case "!=":
                                result = ObjectOperators.NotEqualsOperator(this, objectLeft, objectRight);
                                break;
                            case "!==":
                                result = ObjectOperators.TypeNotEqualsOperator(this, objectLeft, objectRight);
                                break;
                            case "<=":
                                result = ObjectOperators.SmallerOrEqualsOperator(this, objectLeft, objectRight);
                                break;
                            case "<":
                                result = ObjectOperators.SmallerOperator(this, objectLeft, objectRight);
                                break;
                            case ">=":
                                result = ObjectOperators.LargerOrEqualsOperator(this, objectLeft, objectRight);
                                break;
                            case ">":
                                result = ObjectOperators.LargerOperator(this, objectLeft, objectRight);
                                break;
                            case "&&":
                                result = ObjectOperators.AndOperator(this, objectLeft, objectRight);
                                break;
                            case "||":
                                result = ObjectOperators.OrOperator(this, objectLeft, objectRight);
                                break;
                            case "++":
                                result = ObjectOperators.IncrementOperator(this, objectLeft);
                                break;
                            case "--":
                                result = ObjectOperators.DecrementOperator(this, objectLeft);
                                break;
                        }

                        exp = exp.Remove(captureLeft.StartIndex, op.Length + captureLeft.Length + captureRight.Length);
                        exp = exp.Insert(captureLeft.StartIndex, result);

                        var offset = result.Length - (op.Length + captureLeft.Length + captureRight.Length);
                        for (int j = i + 1; j < ops.Length; j++)
                        {
                            ops[j] += offset;
                        }
                    }
                }
            }

            return exp;
        }

        #endregion

        #region Helpers

        private SObject ToScriptObject(string exp)
        {
            exp = exp.Trim();

            // This means it's either an indexer or an array
            if (exp.EndsWith("]"))
            {
                if (!(exp.StartsWith("[") && !exp.Remove(0, 1).Contains("["))) // When there's no "[" besides the start, and it starts with [, then it is an array. Otherwise, do real check.
                {
                    // It is possible that we are having a simple array declaration here.
                    // We check that by looking if we can find a "[" before the expression ends:

                    int depth = 0;
                    int index = exp.Length - 2;
                    int indexerStartIndex = 0;
                    bool hasIndexer = false;
                    StringEscapeHelper escaper = new RightToLeftStringEscapeHelper(exp, index);

                    while (index > 0 && !hasIndexer)
                    {
                        char t = exp[index];
                        escaper.CheckStartAt(index);

                        if (!escaper.IsString)
                        {
                            if (t == ')' || t == '}' || t == ']')
                            {
                                depth++;
                            }
                            else if (t == '(' || t == '{')
                            {
                                depth--;
                            }
                            else if (t == '[')
                            {
                                if (depth == 0)
                                {
                                    if (index > 0)
                                    {
                                        indexerStartIndex = index;
                                        hasIndexer = true;
                                    }
                                }
                                else
                                {
                                    depth--;
                                }
                            }
                        }
                    }

                    if (hasIndexer)
                    {
                        string indexerCode = exp.Remove(0, indexerStartIndex + 1);
                        indexerCode = indexerCode.Remove(indexerCode.Length - 1, 1);

                        string identifier = exp.Remove(indexerStartIndex);

                        SObject statementResult = ExecuteStatement(new ScriptStatement(indexerCode));
                        return ToScriptObject(identifier).GetMember(this, statementResult, true);
                    }
                }
            }

            // Normal object return procedure:

            // Negative number:
            bool isNegative = false;
            if (exp.StartsWith("-"))
            {
                exp = exp.Remove(0, 1);
                isNegative = true;
            }

            double dblResult;
            SObject returnObject;

            if (exp == SObject.LITERAL_NULL)
            {
                returnObject = Null;
            }
            else if (exp == SObject.LITERAL_UNDEFINED)
            {
                returnObject = Undefined;
            }
            else if (exp == SObject.LITERAL_BOOL_FALSE)
            {
                returnObject = CreateBool(false);
            }
            else if (exp == SObject.LITERAL_BOOL_TRUE)
            {
                returnObject = CreateBool(true);
            }
            else if (exp == SObject.LITERAL_NAN)
            {
                returnObject = CreateNumber(double.NaN);
            }
            else if (exp == SObject.LITERAL_THIS)
            {
                returnObject = Context.This;
            }
            else if (SNumber.TryParse(exp, out dblResult))
            {
                returnObject = CreateNumber(dblResult);
            }
            else if (exp.StartsWith("\"") && exp.EndsWith("\"") || exp.StartsWith("\'") && exp.EndsWith("\'"))
            {
                returnObject = CreateString(exp.Remove(exp.Length - 1, 1).Remove(0, 1), true);
            }
            else if (exp.StartsWith("@\"") && exp.EndsWith("\"") || exp.StartsWith("@\'") && exp.EndsWith("\'"))
            {
                returnObject = CreateString(exp.Remove(exp.Length - 1, 1).Remove(0, 2), false);
            }
            else if (exp.StartsWith("{") && exp.EndsWith("}"))
            {
                returnObject = SProtoObject.Parse(this, exp);
            }
            else if (exp.StartsWith("[") && exp.EndsWith("]"))
            {
                returnObject = SArray.Parse(this, exp);
            }
            else if (exp.StartsWith("function") && Regex.IsMatch(exp, REGEX_FUNCTION))
            {
                returnObject = new SFunction(this, exp);
            }
            else if (Context.IsAPIUsing(exp))
            {
                returnObject = Context.GetAPIUsing(exp);
            }
            else if (Context.IsVariable(exp))
            {
                returnObject = Context.GetVariable(exp);
            }
            else if (Context.This.HasMember(this, exp))
            {
                returnObject = Context.This.GetMember(this, CreateString(exp), false);
            }
            else if (Context.IsPrototype(exp))
            {
                returnObject = Context.GetPrototype(exp);
            }
            else if (exp.StartsWith("new "))
            {
                returnObject = Context.CreateInstance(exp);
            }
            else if (exp.StartsWith("$"))
            {
                string strId = exp.Remove(0, 1);
                int id = 0;

                if (int.TryParse(strId, out id) && ObjectBuffer.HasObject(id))
                {
                    returnObject = (SObject)ObjectBuffer.GetObject(id);
                }
                else
                {
                    returnObject = ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_INVALID_TOKEN, new object[] { exp });
                }
            }
            else
            {
                returnObject = ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MESSAGE_REFERENCE_NOT_DEFINED, new object[] { exp });
            }

            if (isNegative)
                returnObject = ObjectOperators.NegateNumber(this, returnObject);

            return returnObject;
        }

        private bool IsDotOperatorDecimalSeparator(string elementLeft, string elementRight)
        {
            return Regex.IsMatch(elementLeft.Trim(), REGEX_NUMLEFTDOT) &&
                   Regex.IsMatch(elementRight.Trim(), REGEX_NUMRIGHTDOT);
        }

        /// <summary>
        /// Returns positions of the given operator in the expression, sorted from left to right.
        /// </summary>
        private int[] GetOperatorPositions(string exp, string op)
        {
            List<int> operators = new List<int>();

            StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(exp, 0);
            int depth = 0;
            int index = 0;

            while (index < exp.Length)
            {
                char t = exp[index];
                escaper.CheckStartAt(index);

                if (!escaper.IsString)
                {
                    if (t == '(' || t == '[' || t == '{')
                    {
                        depth--;
                    }
                    else if (t == ')' || t == ']' || t == '}')
                    {
                        depth++;
                    }

                    if (t == op[0] && depth == 0)
                    {
                        if (op.Length > 1 && index + op.Length - 1 < exp.Length)
                        {
                            bool correctOperator = true;
                            for (int i = 1; i < op.Length; i++)
                            {
                                if (exp[index + i] != op[i])
                                    correctOperator = false;
                            }

                            if (correctOperator)
                                operators.Add(index);
                        }
                        else if (op.Length == 1)
                        {
                            operators.Add(index);
                        }
                    }
                }
                index++;
            }

            return operators.ToArray();
        }

        /// <summary>
        /// Resolves parentheses and adds ".call" to direct function calls on variables.
        /// </summary>
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

        /// <summary>
        /// Builds a function() {} from a lambda statement.
        /// </summary>
        private string BuildLambdaFunction(string lambdaCode)
        {
            string signatureCode = lambdaCode.Remove(lambdaCode.IndexOf("=>")).Trim();
            StringBuilder signatureBuilder = new StringBuilder();

            if (signatureCode != "()")
            {
                string[] signature = signatureCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < signature.Length; i++)
                {
                    if (signatureBuilder.Length > 0)
                        signatureBuilder.Append(',');

                    signatureBuilder.Append(signature[i]);
                }
            }

            string code = lambdaCode.Remove(0, lambdaCode.IndexOf("=>") + 2).Trim();

            return string.Format("function({0}){{return {1};}}", signatureBuilder.ToString(), code);
        }

        /// <summary>
        /// Captures an element right from the starting index.
        /// </summary>
        private ElementCapture CaptureRight(string exp, int index)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return new ElementCapture() { Length = 0, StartIndex = 0, Identifier = "", Depth = 0 };

            string identifier = "";
            bool foundSeparatorChar = false;
            int depth = 0;
            StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(exp, index);

            while (index < exp.Length && !foundSeparatorChar)
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
                            // Check if the '.' is not a decimal separator:
                            if (!Regex.IsMatch(identifier.Trim(), REGEX_NUMLEFTDOT))
                            {
                                foundSeparatorChar = true;
                            }
                        }
                        else if (IDENTIFIER_SEPARATORS.Contains(t))
                        {
                            foundSeparatorChar = true;
                        }
                    }

                    // Append the char to the identifier:
                    if (!foundSeparatorChar)
                    {
                        identifier += t;
                    }
                }

                index++;
            }

            if (foundSeparatorChar)
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
            bool foundSeparatorChar = false;
            int depth = 0;
            StringEscapeHelper escaper = new RightToLeftStringEscapeHelper(exp, index);

            while (index >= 0 && !foundSeparatorChar)
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
                    foundSeparatorChar = true;
                }
                else
                {
                    if (!escaper.IsString && depth == 0)
                    {
                        if (t == '.')
                        {
                            // Check if the '.' is not a decimal separator:
                            if (!Regex.IsMatch(identifier.Trim(), REGEX_NUMRIGHTDOT))
                            {
                                foundSeparatorChar = true;
                            }
                        }
                        else if (IDENTIFIER_SEPARATORS.Contains(t))
                        {
                            foundSeparatorChar = true;
                        }
                    }

                    // Append the char to the identifier:
                    if (!foundSeparatorChar)
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

            if (foundSeparatorChar)
                return new ElementCapture() { StartIndex = index + 2, Length = identifier.Length, Identifier = identifier, Depth = depth };
            else
                return new ElementCapture() { StartIndex = index + 1, Length = identifier.Length, Identifier = identifier, Depth = depth };
        }

        #endregion
    }
}
