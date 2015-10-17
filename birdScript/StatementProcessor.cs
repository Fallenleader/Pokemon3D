using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    public class StatementProcessor
    {
        private static readonly string[] blockControlStatements = new string[] { "if", "else", "else if", "while", "for", "function", "class", "try", "catch" };
        private const int longestBlockControlStatement = 8; //"function"
        private const string blockStatementFirstChars = "iewfct";

        public static ScriptStatement[] GetStatements(string code)
        {
            List<ScriptStatement> statements = new List<ScriptStatement>();

            int index = 0;
            int depth = 0;
            int statementStart = 0;
            bool isStatementFinished = false;
            bool isComment = false;
            bool isBlockStatement = false;
            bool isBlockControlStatement = false;
            bool canBeBlockControlStatement = true;

            StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(code, 0);

            while (index < code.Length)
            {
                char t = code[index];

                escaper.CheckStartAt(index);

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

                        if (t == '(')
                        {
                            depth++;
                            canBeBlockControlStatement = false;
                        }
                        else if (t == ')')
                        {
                            depth--;
                        }
                        else if (t == '{')
                        {
                            depth++;
                            canBeBlockControlStatement = false;
                            if (depth == 1 && string.IsNullOrWhiteSpace(code.Substring(statementStart, index - statementStart)))
                            {
                                isBlockStatement = true;
                            }
                            else if (depth == 1 && isBlockControlStatement)
                            {
                                isStatementFinished = true;
                                index--; //let this char get evaluated again
                                depth = 0; //also reset depth to 0, because we don't go into the statement yet.
                            }
                        }
                        else if (t == '}')
                        {
                            depth--;
                            if (depth == 0 && isBlockStatement)
                            {
                                isStatementFinished = true;
                            }
                        }
                        else if (t == ';' && depth == 0 && !isBlockControlStatement && !isBlockStatement)
                        {
                            isStatementFinished = true;
                        }
                        else if (canBeBlockControlStatement && !isBlockControlStatement && !isBlockStatement)
                        {
                            string startOfStatement = code.Substring(statementStart, index - statementStart + 1).TrimStart();

                            if (startOfStatement.Length > 0)
                            {
                                if (blockStatementFirstChars.Contains(startOfStatement[0]))
                                {
                                    if (blockControlStatements.Contains(startOfStatement))
                                    {
                                        isBlockControlStatement = true;
                                    }
                                    else
                                    {
                                        // If the current statement is already longer than the longest possible block control statement, do not look for it anymore.
                                        // This saves us doing the substring and trim checks.
                                        if (startOfStatement.Length >= longestBlockControlStatement)
                                        {
                                            canBeBlockControlStatement = false;
                                        }
                                    }
                                }
                                else
                                {
                                    canBeBlockControlStatement = false;
                                }
                            }
                        }

                    }
                    else
                    {
                        // check if the block comment is ending (*/):
                        if (t == '*' && index + 1 < code.Length && code[index + 1] == '/')
                        {
                            isComment = false;
                            index++; // Jump over / char.
                        }
                    }
                }

                if (isStatementFinished)
                {
                    string statement = code.Substring(statementStart, index - statementStart + 1).Trim();
                    if (statement.Length > 0)
                    {
                        statement = statement.TrimEnd(new char[] { ';' });
                        statements.Add(new ScriptStatement(statement, GetStatementType(statement, isBlockControlStatement, isBlockStatement)));
                    }

                    isStatementFinished = false;
                    isBlockStatement = false;
                    isBlockControlStatement = false;
                    canBeBlockControlStatement = true;

                    statementStart = index + 1;
                }

                index++;
            }

            if (index != statementStart)
            {
                string statement = code.Substring(statementStart, index - statementStart).Trim();
                if (statement.Length > 0)
                {
                    statements.Add(new ScriptStatement(statement, GetStatementType(statement, false, false)));
                }
            }

            return statements.ToArray();
        }

        /// <summary>
        /// Returns the correct statement type for an expression.
        /// </summary>
        internal static StatementType GetStatementType(string code, bool canBeBlockControlStatement, bool isBlockStatement)
        {
            if (canBeBlockControlStatement)
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

            if (!isBlockStatement)
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
            else
            {
                // For block statements:
                return StatementType.Executable;
            }
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
