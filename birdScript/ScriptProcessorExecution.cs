﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;
using birdScript.Adapters;

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
                    return ExecuteAssignment(statement);
                case StatementType.For:
                    return ExecuteFor(statement);
                case StatementType.Function:
                    return ExecuteFunction(statement);
                case StatementType.Class:
                    return ExecuteClass(statement);
                case StatementType.Link:
                    return ExecuteLink(statement);
                case StatementType.Continue:
                    return ExecuteContinue(statement);
                case StatementType.Break:
                    return ExecuteBreak(statement);
                case StatementType.Throw:
                    return ExecuteThrow(statement);
                case StatementType.Try:
                    return ExecuteTry(statement);
                case StatementType.Catch:
                    return ExecuteCatch(statement);
                case StatementType.Finally:
                    return ExecuteFinally(statement);
            }

            return null;
        }

        private SObject ExecuteCatch(ScriptStatement statement)
        {
            // The script processor can only reach this, when a catch statement exists without a try statement:
            return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CATCH_WITHOUT_TRY);
        }

        private SObject ExecuteFinally(ScriptStatement statement)
        {
            // The script processor can only reach this, when a finally statement exists without a try statement:
            return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_FINALLY_WITHOUT_TRY);
        }

        private SObject ExecuteTry(ScriptStatement statement)
        {
            string exp = statement.Code;

            if (exp != "try")
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_BEFORE_TRY);

            _index++;
            if (_statements.Length > _index)
            {
                ScriptStatement executeStatement = _statements[_index];
                bool encounteredError = false;
                bool foundCatch = false;
                bool foundFinally = false;
                bool foundMatchingCatch = false;
                SObject errorObject = null;
                SObject returnObject = Undefined;

                try
                {
                    returnObject = ExecuteStatement(executeStatement);
                }
                catch (ScriptException ex)
                {
                    encounteredError = true;
                    errorObject = ex.ErrorObject;
                }
                
                if (encounteredError)
                {
                    bool endedCatchSearch = false;
                    int findCatchIndex = _index + 1;
                    while (findCatchIndex < _statements.Length && !endedCatchSearch)
                    {
                        if (_statements[findCatchIndex].StatementType == StatementType.Catch)
                        {
                            _index = findCatchIndex + 1;

                            if (_statements.Length > _index)
                            {
                                if (!foundMatchingCatch)
                                {
                                    ScriptStatement catchExecuteStatement = _statements[_index];
                                    foundCatch = true;

                                    string catchCode = _statements[findCatchIndex].Code;
                                    string errorVarName = "";

                                    if (catchCode != "catch")
                                    {
                                        catchCode = catchCode.Remove(0, "catch".Length).Trim().Remove(0, 1);
                                        catchCode = catchCode.Remove(catchCode.Length - 1, 1);
                                        errorVarName = catchCode.Trim();
                                    }

                                    if (Regex.IsMatch(catchCode, REGEX_CATCHCONDITION))
                                    {
                                        errorVarName = catchCode.Remove(catchCode.IndexOf(" "));
                                        string conditionCode = catchCode.Remove(0, catchCode.IndexOf("if") + 3);

                                        ScriptProcessor processor = new ScriptProcessor(Context, GetLineNumber());
                                        processor.Context.AddVariable(errorVarName, errorObject);

                                        SObject conditionResult = processor.ExecuteStatement(new ScriptStatement(conditionCode));

                                        if (conditionResult is SBool)
                                            foundMatchingCatch = ((SBool)conditionResult).Value;
                                        else
                                            foundMatchingCatch = conditionResult.ToBool(this).Value;

                                        if (foundMatchingCatch)
                                            returnObject = processor.ExecuteStatement(catchExecuteStatement);
                                    }
                                    else
                                    {
                                        foundMatchingCatch = true;
                                        ScriptProcessor processor = new ScriptProcessor(Context, GetLineNumber());
                                        processor.Context.AddVariable(errorVarName, errorObject);
                                        returnObject = processor.ExecuteStatement(catchExecuteStatement);
                                    }
                                }
                            }
                            else
                            {
                                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, "end of script" );
                            }
                        }
                        else
                        {
                            // end search if different statement type appears:
                            endedCatchSearch = true;
                        }

                        findCatchIndex += 2;
                    }
                }
                else
                {
                    int findCatchIndex = _index + 1;
                    while (findCatchIndex < _statements.Length)
                    {
                        if (_statements[findCatchIndex].StatementType == StatementType.Catch)
                        {
                            foundCatch = true;
                            _index = findCatchIndex + 1;
                        }
                        else
                        {
                            findCatchIndex = _statements.Length;
                        }

                        findCatchIndex += 2;
                    }
                }

                // if no matching catch was found when an error occurred, it was not caught: throw it!
                if (encounteredError && !foundMatchingCatch)
                    return ErrorHandler.ThrowError(errorObject);

                // now, try to find finally statement:
                int findFinallyIndex = _index + 1;
                while (findFinallyIndex < _statements.Length && !foundFinally)
                {
                    if (_statements[findFinallyIndex].StatementType == StatementType.Finally)
                    {
                        _index = findFinallyIndex + 1;

                        if (_statements.Length > _index)
                        {
                            ScriptStatement finallyExecuteStatement = _statements[_index];
                            foundFinally = true;

                            returnObject = ExecuteStatement(finallyExecuteStatement);
                        }
                        else
                        {
                            return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  "end of script" );
                        }
                    }
                    else
                    {
                        findFinallyIndex = _statements.Length;
                    }

                    findFinallyIndex += 2;
                }

                if (!foundCatch && !foundFinally) // when no catch or finally block has been found, throw an error.
                    return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_CATCH_OR_FINALLY);
                else
                    return returnObject;
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, "end of script" );
            }
        }

        private SObject ExecuteThrow(ScriptStatement statement)
        {
            string exp = statement.Code;

            if (exp == "throw")
            {
                return ErrorHandler.ThrowError(ErrorType.UserError, ErrorHandler.MESSAGE_USER_ERROR);
            }
            else
            {
                exp = exp.Remove(0, "throw ".Length).Trim();

                var errorObject = ExecuteStatement(new ScriptStatement(exp));

                // Set the line number if the object is an error object:
                if (errorObject.TypeOf() == SObject.LITERAL_TYPE_ERROR)
                    ((SError)errorObject).SetMember(ErrorPrototype.MEMBER_NAME_LINE, CreateNumber(GetLineNumber()));

                return ErrorHandler.ThrowError(errorObject);
            }
        }

        private SObject ExecuteContinue(ScriptStatement statement)
        {
            _continueIssued = true;
            return Undefined;
        }

        private SObject ExecuteBreak(ScriptStatement statement)
        {
            _breakIssued = true;
            return Undefined;
        }

        private SObject ExecuteLink(ScriptStatement statement)
        {
            if (Context.HasCallback(CallbackType.ScriptPipeline))
            {
                string exp = statement.Code;
                exp = exp.Remove(0, "link ".Length).Trim();

                var callback = (DScriptPipeline)Context.GetCallback(CallbackType.ScriptPipeline);
                Task<string> task = Task<string>.Factory.StartNew(() => callback(this, exp));
                task.Wait();

                string code = task.Result;

                ScriptStatement[] statements = StatementProcessor.GetStatements(this, code);

                // Convert the current statements into a list, so we can modify them.
                List<ScriptStatement> tempStatements = _statements.ToList();
                // Remove the "link" statement, because we don't want to step into it again if we are in a loop.
                tempStatements.RemoveAt(_index);

                // Insert class, using and link statements right after the current statement.
                int insertIndex = _index;

                for (int i = 0; i < statements.Length; i++)
                {
                    if (statements[i].StatementType == StatementType.Class)
                    {
                        // The class statement needs its body, so we add the class statement and the one afterwards:
                        if (statements.Length > i + 1)
                        {
                            tempStatements.Insert(insertIndex, statements[i]);
                            tempStatements.Insert(insertIndex + 1, statements[i + 1]);

                            insertIndex += 2;
                        }
                        else
                        {
                            ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  "end of script" );
                        }
                    }
                    else if (statements[i].StatementType == StatementType.Using || statements[i].StatementType == StatementType.Link)
                    {
                        tempStatements.Insert(insertIndex, statements[i]);
                        insertIndex += 1;
                    }
                }

                // Convert the temp statement list back and reduce the index by one, because we deleted the current statement.
                _statements = tempStatements.ToArray();
                _index--;

                return Undefined;
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.APIError, ErrorHandler.MESSAGE_API_NOT_SUPPORTED);
            }
        }

        private SObject ExecuteClass(ScriptStatement statement)
        {
            string exp = statement.Code;

            // The function's body is the next statement:

            _index++;

            if (_index < _statements.Length)
            {
                ScriptStatement classBodyStatement = _statements[_index];

                if (classBodyStatement.IsCompoundStatement)
                {
                    exp += classBodyStatement.Code;

                    Prototype prototype = (Prototype)Prototype.Parse(this, exp);
                    Context.AddPrototype(prototype);

                    return prototype;
                }
                else
                {
                    return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_COMPOUND, classBodyStatement.Code[0] );
                }
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, "end of script" );
            }
        }

        private SObject ExecuteFunction(ScriptStatement statement)
        {
            // function <name> ()
            string exp = statement.Code;
            exp = exp.Remove(0, "function".Length).Trim();
            string functionName = exp.Remove(exp.IndexOf("("));

            if (!IsValidIdentifier(functionName))
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            string functionExpression = "function " + exp.Remove(0, exp.IndexOf("("));

            // The function's body is the next statement:

            _index++;

            if (_index < _statements.Length)
            {
                ScriptStatement functionBodyStatement = _statements[_index];

                string functionBody = functionBodyStatement.Code;
                if (!functionBodyStatement.IsCompoundStatement)
                    functionBody = "{" + functionBody + "}";

                functionExpression += functionBody;

                var function = new SFunction(this, functionExpression);
                Context.AddVariable(functionName, function);

                return function;
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  "end of script" );
            }
        }

        private SObject ExecuteFor(ScriptStatement statement)
        {
            string exp = statement.Code;

            string forCode = exp.Remove(0, exp.IndexOf("for") + "for".Length).Trim().Remove(0, 1); // Remove "for" and "(".
            forCode = forCode.Remove(forCode.Length - 1, 1); // Remove ")".

            ScriptStatement[] forStatements = StatementProcessor.GetStatements(this, forCode);

            if (forStatements.Length == 0)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  ")" );
            else if (forStatements.Length == 1)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_FOR_INITIALIZER);
            else if (forStatements.Length == 2)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_FOR_CONDITION);
            else if (forStatements.Length > 3)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_FOR_CONTROL);

            var processor = new ScriptProcessor(Context, GetLineNumber());

            ScriptStatement forInitializer = forStatements[0];
            ScriptStatement forCondition = forStatements[1];
            ScriptStatement forControl = forStatements[2];

            if (forInitializer.Code.Length > 0)
                processor.ExecuteStatement(forInitializer);

            _index++;

            if (_statements.Length > _index)
            {
                bool stayInFor = true;
                var executeStatement = _statements[_index];
                var returnObject = Undefined;

                while (stayInFor)
                {
                    if (forCondition.Code.Length > 0)
                    {
                        SObject conditionResult = processor.ExecuteStatement(forCondition);

                        if (conditionResult is SBool)
                            stayInFor = ((SBool)conditionResult).Value;
                        else
                            stayInFor = conditionResult.ToBool(this).Value;
                    }

                    if (stayInFor)
                    {
                        returnObject = processor.ExecuteStatement(executeStatement);

                        if (processor._returnIssued || processor._breakIssued)
                        {
                            _breakIssued = false;
                            _returnIssued = processor._returnIssued;
                            stayInFor = false;
                        }
                        else if (forControl.Code.Length > 0)
                        {
                            processor.ExecuteStatement(forControl);
                        }
                    }
                }

                return returnObject;
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  "end of script" );
            }
        }

        private SObject ExecuteAssignment(ScriptStatement statement)
        {
            string exp = statement.Code;

            string leftSide = "";
            string rightSide = "";
            string assignmentOperator = "";

            // Get left and right side of the assignment:
            {
                int depth = 0;
                int index = 0;
                StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(exp, 0);

                while (index < exp.Length && assignmentOperator.Length == 0)
                {
                    char t = exp[index];
                    escaper.CheckStartAt(index);

                    if (!escaper.IsString)
                    {
                        if (t == '(' || t == '[' || t == '{')
                        {
                            depth++;
                        }
                        else if (t == ')' || t == ']' || t == '}')
                        {
                            depth--;
                        }
                        else if (t == '=' && depth == 0)
                        {
                            char previous = ' ';
                            if (index > 0)
                                previous = exp[index - 1];

                            if (previous == '+' || previous == '-' || previous == '/' || previous == '*')
                            {
                                assignmentOperator = previous.ToString();
                                leftSide = exp.Substring(0, index - 1).TrimEnd();
                            }
                            else
                            {
                                assignmentOperator = "=";
                                leftSide = exp.Substring(0, index).TrimEnd();
                            }

                            rightSide = exp.Substring(index + 1).TrimStart();
                        }
                    }

                    index++;
                }
            }

            // This means it's a function call, which cannot be assigned to:
            if (leftSide.EndsWith(")") || leftSide.Length == 0)
                return ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MESSAGE_REFERENCE_INVALID_ASSIGNMENT_LEFT);

            SObject memberHost = null;
            SObject accessor = null;
            SObject value = null;
            bool isIndexer = false;
            string host = "";
            string member = "";

            if (leftSide.EndsWith("]"))
            {
                int indexerStartIndex = 0;
                int index = leftSide.Length - 1;
                int depth = 0;

                StringEscapeHelper escaper = new RightToLeftStringEscapeHelper(leftSide, index);
                while (index > 0 && !isIndexer)
                {
                    char t = leftSide[index];
                    escaper.CheckStartAt(index);

                    if (!escaper.IsString)
                    {
                        if (t == '(' || t == '{')
                        {
                            depth--;
                        }
                        else if (t == ')' || t == ']' || t == '}')
                        {
                            depth++;
                        }
                        else if (t == '[')
                        {
                            depth--;
                            if (depth == 0 && index > 0)
                            {
                                isIndexer = true;
                                indexerStartIndex = index;
                            }
                        }
                    }

                    index--;
                }

                if (isIndexer)
                {
                    member = leftSide.Substring(indexerStartIndex + 1);
                    member = member.Remove(member.Length - 1, 1);
                    host = leftSide.Remove(indexerStartIndex);
                }
            }
            else
            {
                bool foundMember = false;

                if (leftSide.Contains("."))
                {
                    int index = leftSide.Length - 1;
                    int depth = 0;
                    StringEscapeHelper escaper = new RightToLeftStringEscapeHelper(leftSide, index);

                    while (index > 0 && !foundMember)
                    {
                        char t = leftSide[index];
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
                            else if (t == '.' && depth == 0)
                            {
                                foundMember = true;
                                host = leftSide.Substring(0, index);
                                member = leftSide.Remove(0, index + 1);
                            }
                        }

                        index--;
                    }
                }

                if (!foundMember)
                {
                    host = SObject.LITERAL_THIS;
                    member = leftSide;
                }
            }

            // When it's an indexer, we parse it as statement:
            if (isIndexer)
            {
                accessor = SObject.Unbox(ExecuteStatement(new ScriptStatement(member)));
            }
            else
            {
                accessor = CreateString(member);
            }
            
            memberHost = ExecuteStatement(new ScriptStatement(host));
            value = SObject.Unbox(ExecuteStatement(new ScriptStatement(rightSide)));

            if (assignmentOperator == "=")
            {
                memberHost.SetMember(this, accessor, isIndexer, value);
            }
            else
            {
                var memberContent = memberHost.GetMember(this, accessor, isIndexer);

                string result = "";

                switch (assignmentOperator)
                {
                    case "+":
                        result = ObjectOperators.AddOperator(this, memberContent, value);
                        break;
                    case "-":
                        result = ObjectOperators.SubtractOperator(this, memberContent, value);
                        break;
                    case "*":
                        result = ObjectOperators.MultiplyOperator(this, memberContent, value);
                        break;
                    case "/":
                        result = ObjectOperators.DivideOperator(this, memberContent, value);
                        break;
                }

                memberHost.SetMember(this, accessor, isIndexer, ToScriptObject(result));
            }

            return value;
        }

        private SObject ExecuteExecutable(ScriptStatement statement)
        {
            if (statement.IsCompoundStatement)
            {
                ScriptProcessor processor = new ScriptProcessor(Context, GetLineNumber());

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
                else if (exp == SObject.LITERAL_UNDEFINED || exp == "")
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
                    exp = EvaluateOperator(exp, ".");
                if (exp.Contains("++"))
                    exp = EvaluateOperator(exp, "++");
                if (exp.Contains("--"))
                    exp = EvaluateOperator(exp, "--");
                if (exp.Contains("!"))
                    exp = EvaluateReverseBool(exp);
                if (exp.Contains("**"))
                    exp = EvaluateOperator(exp, "**");
                if (exp.Contains("*"))
                    exp = EvaluateOperator(exp, "*");
                if (exp.Contains("/"))
                    exp = EvaluateOperator(exp, "/");
                if (exp.Contains("%"))
                    exp = EvaluateOperator(exp, "%");
                if (exp.Contains("+"))
                    exp = EvaluateOperator(exp, "+");
                if (exp.Contains("-"))
                    exp = EvaluateOperator(exp, "-");
                if (exp.Contains("<="))
                    exp = EvaluateOperator(exp, "<=");
                if (exp.Contains(">="))
                    exp = EvaluateOperator(exp, ">=");
                if (exp.Contains("<"))
                    exp = EvaluateOperator(exp, "<");
                if (exp.Contains(">"))
                    exp = EvaluateOperator(exp, ">");
                if (exp.Contains("==="))
                    exp = EvaluateOperator(exp, "===");
                if (exp.Contains("!=="))
                    exp = EvaluateOperator(exp, "!==");
                if (exp.Contains("=="))
                    exp = EvaluateOperator(exp, "==");
                if (exp.Contains("!="))
                    exp = EvaluateOperator(exp, "!=");
                if (exp.Contains("&&"))
                    exp = EvaluateOperator(exp, "&&");
                if (exp.Contains("||"))
                    exp = EvaluateOperator(exp, "||");

                return ToScriptObject(exp);
            }
        }

        private SObject ExecuteIf(ScriptStatement statement)
        {
            string exp = statement.Code;

            string condition = exp.Remove(0, exp.IndexOf("if") + "if".Length).Trim().Remove(0, 1); // Remove "if" and "(".
            condition = condition.Remove(condition.Length - 1, 1).Trim(); // Remove ")".

            if (condition.Length == 0)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, ")" );

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
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, "end of script");
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
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, "keyword \'else\'" );

            _index++;

            if (_statements.Length > _index)
            {
                var executeStatement = _statements[_index];
                return ExecuteStatement(executeStatement);
            }
            else
            {
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  "end of script" );
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
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, "keyword \'else if\'" );

            return ExecuteIf(statement);
        }

        private SObject ExecuteUsing(ScriptStatement statement)
        {
            string exp = statement.Code;

            string identifier = exp.Remove(0, "using ".Length).Trim();

            if (!IsValidIdentifier(identifier))
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            var apiUsing = new SAPIUsing(identifier);

            Context.AddAPIUsing(apiUsing);
            return apiUsing;
        }

        private SObject ExecuteVar(ScriptStatement statement)
        {
            string exp = statement.Code;

            string identifier = exp.Remove(0, "var ".Length).Trim();
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
            condition = condition.Remove(condition.Length - 1, 1).Trim(); // Remove ")".

            if (condition.Length == 0)
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, ")" );

            _index++;

            if (_statements.Length > _index)
            {
                bool stayInWhile = true;
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
                return ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION,  "end of script" );
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
