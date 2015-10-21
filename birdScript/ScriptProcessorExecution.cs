using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;

namespace birdScript
{
    partial class ScriptProcessor
    {
        internal SObject ExecuteStatement(ScriptStatement statement)
        {
            switch (statement.StatementType)
            {
                case StatementType.Executable:
                    return ExecuteExecutable(statement);
                case StatementType.If:
                    return ExecuteIf(statement);
                case StatementType.Else:
                    return ExecuteElse(statement);
                case StatementType.ElseIf:
                    return ExecuteElseIf(statement);
                case StatementType.Using:
                    return ExecuteUsing(statement);
                case StatementType.Var:
                    return ExecuteVar(statement);
                case StatementType.While:
                    return ExecuteWhile(statement);
                case StatementType.Return:
                    return ExecuteReturn(statement);
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

        private SObject ExecuteIf(ScriptStatement statement)
        {
            string exp = statement.Code;

            string condition = exp.Remove(0, exp.IndexOf("if") + "if".Length).Trim().Remove(0, 1); // Remove "if" and "(".
            condition = condition.Remove(condition.Length - 1, 1); // Remove ")".

            SObject conditionResult = ExecuteStatement(new ScriptStatement(condition));
            statement.StatementResult = conditionResult;

            bool conditionEval;
            if (conditionResult is SBool)
                conditionEval = ((SBool)conditionResult).Value;
            else
                conditionEval = conditionResult.ToBool(this).Value;

            _index++;

            if (_statements.Length > _index)
            {
                var executeStatement = _statements[_index];

                if (conditionEval)
                {
                    SObject returnObject = ExecuteStatement(executeStatement);

                    // Jump over all "else if" and "else" statements that follow this if / else if:

                    int searchIndex = _index + 1;
                    bool foundIfs = true;

                    while (searchIndex < _statements.Length && foundIfs)
                    {
                        if (_statements[searchIndex].StatementType == StatementType.ElseIf || _statements[searchIndex].StatementType == StatementType.Else)
                            _index += 2;
                        else
                            foundIfs = false;

                        searchIndex += 2;
                    }

                    return returnObject;
                }
                else
                {
                    return Undefined;
                }
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, new object[] { "end of script" });
            }
        }

        private SObject ExecuteElse(ScriptStatement statement)
        {
            // Search for an if statement:
            int searchIndex = _index - 2;
            bool foundIf = false;

            while (searchIndex >= 0 && !foundIf)
            {
                if (_statements[searchIndex].StatementType == StatementType.If)
                    foundIf = true;

                searchIndex -= 2;
            }

            if (!foundIf)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, new object[] { "keyword \'else\'" });

            _index++;

            if (_statements.Length > _index)
            {
                var executeStatement = _statements[_index];
                return ExecuteStatement(executeStatement);
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, new object[] { "end of script" });
            }
        }

        private SObject ExecuteElseIf(ScriptStatement statement)
        {
            // Search for an if statement:
            int searchIndex = _index - 2;
            bool foundIf = false;

            while (searchIndex >= 0 && !foundIf)
            {
                if (_statements[searchIndex].StatementType == StatementType.If)
                    foundIf = true;

                searchIndex -= 2;
            }

            if (!foundIf)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, new object[] { "keyword \'else if\'" });

            return ExecuteIf(statement);
        }

        private SObject ExecuteUsing(ScriptStatement statement)
        {
            string exp = statement.Code;

            string identifier = exp.Remove(0, "using".Length).Trim();

            if (!IsValidIdentifier(identifier))
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            var apiUsing = new SAPIUsing(identifier);

            Context.AddAPIUsing(apiUsing);
            return apiUsing;
        }

        private SObject ExecuteVar(ScriptStatement statement)
        {
            string exp = statement.Code;

            string identifier = exp.Remove(0, "var".Length).Trim();
            SObject data = Undefined;

            if (identifier.Contains("="))
            {
                string assignment = identifier.Remove(0, identifier.IndexOf("=") + 1).Trim();
                identifier = identifier.Remove(identifier.IndexOf("=")).Trim();

                data = SObject.Unbox(ExecuteStatement(new ScriptStatement(assignment)));
            }

            if (!IsValidIdentifier(identifier))
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            SVariable variable = new SVariable(identifier, data);
            Context.AddVariable(variable);

            return variable;
        }

        private SObject ExecuteWhile(ScriptStatement statement)
        {
            string exp = statement.Code;

            string condition = exp.Remove(0, exp.IndexOf("while") + "while".Length).Trim().Remove(0, 1); // Remove "while" and "(".
            condition = condition.Remove(condition.Length - 1, 1); // Remove ")".

            bool stayInWhile = true;

            _index++;

            if (_statements.Length > _index)
            {
                var executeStatement = _statements[_index];
                var returnObject = Undefined;

                while (stayInWhile)
                {
                    SObject conditionResult = ExecuteStatement(new ScriptStatement(condition));

                    if (conditionResult is SBool)
                        stayInWhile = ((SBool)conditionResult).Value;
                    else
                        stayInWhile = conditionResult.ToBool(this).Value;

                    if (stayInWhile)
                    {
                        returnObject = ExecuteStatement(executeStatement);

                        if (_returnIssued || _breakIssued)
                        {
                            _breakIssued = false;
                            stayInWhile = false;
                        }
                    }
                }

                return returnObject;
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, new object[] { "end of script" });
            }
        }

        private SObject ExecuteReturn(ScriptStatement statement)
        {
            string exp = statement.Code;

            if (exp == "return")
            {
                _returnIssued = true;
                return Undefined;
            }
            else
            {
                exp = exp.Remove(0, "return".Length);

                var returnObject = ExecuteStatement(new ScriptStatement(exp));
                _returnIssued = true;

                return returnObject;
            }
        }
    }
}
