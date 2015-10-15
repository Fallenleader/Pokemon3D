using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    internal enum StatementType
    {
        Executable,
        If,
        Else,
        ElseIf,
        Using,
        Var,
        While,
        Return,
        Assignment,
        For,
        Function,
        Class,
        Link,
        Continue,
        Break,
        Throw,
        Try,
        Catch,
        Finally
    }

    internal class ScriptStatement
    {
        public string Code { get; }
        public StatementType StatementType { get; }

        public SObject StatementResult;

        public ScriptStatement(string code, StatementType statementType)
        {
            Code = code.Trim();

            if (statementType == StatementType.Executable)
            {
                if (Code.StartsWith("var "))
                {
                    StatementType = StatementType.Var;
                }
                else if (Code.StartsWith("using "))
                {
                    StatementType = StatementType.Using;
                }
                else if (Code.StartsWith("link "))
                {
                    StatementType = StatementType.Link;
                }
                else if (Code.StartsWith("return ") || Code == "return")
                {
                    StatementType = StatementType.Return;
                }
                else if (Code == "continue")
                {
                    StatementType = StatementType.Continue;
                }
                else if (Code == "break")
                {
                    StatementType = StatementType.Break;
                }
                else if (Code.StartsWith("throw "))
                {
                    StatementType = StatementType.Throw;
                }
                else if (IsAssignment(Code))
                {
                    StatementType = StatementType.Assignment;
                }
            }
        }

        /// <summary>
        /// Returns if the expression is an assignment statement.
        /// </summary>
        private static bool IsAssignment(string code)
        {
            if (!StringEscapeHelper.ContainsWithoutStrings(code, "="))
            {
                return false;
            }
            else
            {
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
