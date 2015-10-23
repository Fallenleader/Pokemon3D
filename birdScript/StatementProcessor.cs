using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    class StatementProcessor
    {
        internal static readonly string[] controlStatements = new string[] { "if", "else", "else if", "while", "for", "function", "class", "try", "catch" };

        private const string OBJECT_DISCOVER_TOKEN = "+*-/&|<>.[(;";

        internal static ScriptStatement[] GetStatements(ScriptProcessor processor, string code)
        {
            List<ScriptStatement> statements = new List<ScriptStatement>();

            StringBuilder statement = new StringBuilder();

            int index = 0;
            int depth = 0;
            bool isComment = false;
            bool isControlStatement = false; // If the current statement is a control statement.
            bool isCompoundStatement = false; // If the current statement is bunch of statements wrapped in { ... }

            StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(code, 0);

            while (index < code.Length)
            {
                char t = code[index];

                if (!isComment)
                    escaper.CheckStartAt(index);
                else
                    escaper.JumpTo(index);

                if (!escaper.IsString)
                {
                    // Check if a block comment is starting (/*):
                    if (!isComment && t == '/' && index + 1 < code.Length && code[index + 1] == '*')
                    {
                        isComment = true;
                        index++; // Jump over * char.
                    }

                    if (!isComment)
                    {
                        // Check if a line comment is starting (//):
                        if (t == '/' && index + 1 < code.Length && code[index + 1] == '/')
                        {
                            // We jump to the end of the line and ignore everything between the current index and the end of the line:
                            if (code.IndexOf("\n", index + 1) > -1)
                                index = code.IndexOf("\n", index + 1) + 1;
                            else
                                index = code.Length;

                            continue;
                        }

                        statement.Append(t);

                        if (t == '(')
                        {
                            depth++;
                        }
                        else if (t == ')')
                        {
                            depth--;

                            if (isControlStatement)
                            {
                                string s = statement.ToString().Trim();
                                if (s.StartsWith("if") || s.StartsWith("else if") || s.StartsWith("function") || s.StartsWith("for") || s.StartsWith("while") || s.StartsWith("catch"))
                                {
                                    statements.Add(new ScriptStatement(s, GetStatementType(s, true)));
                                    statement.Clear();

                                    isControlStatement = false;
                                }
                            }
                        }
                        else if (t == '{')
                        {
                            depth++;

                            if (depth == 1)
                            {
                                string s = statement.ToString().Trim();
                                if (isControlStatement)
                                {
                                    s = s.Remove(s.Length - 1, 1).Trim();
                                    statements.Add(new ScriptStatement(s, GetStatementType(s, true)));

                                    statement.Clear();
                                    statement.Append('{');
                                    isCompoundStatement = true;
                                    isControlStatement = false;
                                }
                                else
                                {
                                    if (s == "{")
                                    {
                                        isCompoundStatement = true;
                                    }
                                }
                            }
                        }
                        else if (t == '}')
                        {
                            depth--;
                            if (depth == 0 && isCompoundStatement)
                            {
                                // This could also be an object declaration...
                                // In the case that the statement started with "{" (example statement: {} + []), this will happen.
                                // To check if this is in fact an object, we look right and see if there is:
                                //   - an operator => object ("+*-/&|<>.[(")
                                //   - nothing => statement

                                bool foundOperator = false;
                                int charFindIndex = index + 1;

                                while (!foundOperator && charFindIndex < code.Length)
                                {
                                    char testChar = code[charFindIndex];
                                    if (OBJECT_DISCOVER_TOKEN.Contains(testChar))
                                    {
                                        foundOperator = true;
                                    }
                                    else if (!char.IsWhiteSpace(testChar)) // We found something that is not an operator or whitespace, so this is the end of a compound statement.
                                    {
                                        charFindIndex = code.Length;
                                    }
                                    
                                    charFindIndex++;
                                }

                                if (!foundOperator)
                                {
                                    string s = statement.ToString().Trim();
                                    statements.Add(new ScriptStatement(s, StatementType.Executable) { IsCompoundStatement = true });
                                    statement.Clear();
                                }

                                isCompoundStatement = false;
                            }
                        }
                        else if (t == ';' && depth == 0)
                        {
                            string s = statement.ToString().Trim().TrimEnd(new char[] { ';' });
                            statements.Add(new ScriptStatement(s, GetStatementType(s, false)));
                            statement.Clear();

                        }
                        else if (!isCompoundStatement && !isControlStatement)
                        {
                            string s = statement.ToString().TrimStart();
                            if (controlStatements.Contains(s))
                            {
                                isControlStatement = true;
                                if (s.StartsWith("else"))
                                {
                                    if (index + 3 < code.Length)
                                    {
                                        string check = code.Substring(index + 1, 3);
                                        if (check != " if")
                                        {
                                            statements.Add(new ScriptStatement("else", StatementType.Else));
                                            statement.Clear();
                                            isControlStatement = false;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        // Check if a block comment is ending (/*):
                        if (t == '*' && index + 1 < code.Length && code[index + 1] == '/')
                        {
                            isComment = false;
                            index++; // Jump over / char.
                        }
                    }
                }
                else
                {
                    statement.Append(t);
                }

                index++;
            }

            if (isCompoundStatement)
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_END_OF_COMPOUND_STATEMENT);

            if (isComment)
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_UNTERMINATED_COMMENT);

            if (isControlStatement)
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_EXPECTED_EXPRESSION, new object[] { "end of script" });

            // an executable statement not closed with ";" is getting added here:
            string leftOver = statement.ToString().Trim();
            if (leftOver.Length > 0)
            {
                statements.Add(new ScriptStatement(leftOver, GetStatementType(leftOver, false)));
            }

            return statements.ToArray();
        }

        /// <summary>
        /// Returns the correct statement type for an expression.
        /// </summary>
        internal static StatementType GetStatementType(string code, bool isControlStatement)
        {
            if (isControlStatement)
            {
                if (code.StartsWith("if"))
                {
                    return StatementType.If;
                }
                else if (code.StartsWith("else if"))
                {
                    return StatementType.ElseIf;
                }
                else if (code.StartsWith("else"))
                {
                    return StatementType.Else;
                }
                else if (code.StartsWith("while"))
                {
                    return StatementType.While;
                }
                else if (code.StartsWith("for"))
                {
                    return StatementType.For;
                }
                else if (code.StartsWith("function"))
                {
                    return StatementType.Function;
                }
                else if (code.StartsWith("class"))
                {
                    return StatementType.Class;
                }
                else if (code.StartsWith("try"))
                {
                    return StatementType.Try;
                }
                else if (code.StartsWith("catch"))
                {
                    return StatementType.Catch;
                }
            }
            else
            {
                if (code.StartsWith("var "))
                {
                    return StatementType.Var;
                }
                else if (code.StartsWith("using "))
                {
                    return StatementType.Using;
                }
                else if (code.StartsWith("link "))
                {
                    return StatementType.Link;
                }
                else if (code.StartsWith("return ") || code == "return")
                {
                    return StatementType.Return;
                }
                else if (code == "continue")
                {
                    return StatementType.Continue;
                }
                else if (code == "break")
                {
                    return StatementType.Break;
                }
                else if (code.StartsWith("throw "))
                {
                    return StatementType.Throw;
                }
                else if (IsAssignmentStatement(code))
                {
                    return StatementType.Assignment;
                }
                else
                {
                    return StatementType.Executable;
                }
            }
            return StatementType.Executable;
        }

        /// <summary>
        /// Returns if the expression is an assignment statement.
        /// </summary>
        private static bool IsAssignmentStatement(string code)
        {
            if (!StringEscapeHelper.ContainsWithoutStrings(code, "="))
            {
                return false;
            }
            else
            {
                // Replace "=" that are not the assignment operator with placeholders:
                code = code.Replace("===", "---");
                code = code.Replace("!==", "---");
                code = code.Replace("==", "--");
                code = code.Replace("!=", "--");
                code = code.Replace("=>", "--");

                return StringEscapeHelper.ContainsWithoutStrings(code, "=");
            }
        }
    }
}
