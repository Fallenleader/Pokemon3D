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

    /// <summary>
    /// A statement for the <see cref="ScriptProcessor"/> to execute.
    /// </summary>
    internal class ScriptStatement
    {
        internal string Code { get; }
        internal StatementType StatementType { get; }

        internal SObject StatementResult;

        internal bool IsCompoundStatement { get; set; }

        internal ScriptStatement(string code)
        {
            Code = code.Trim();
            StatementType = StatementProcessor.GetStatementType(Code, false);
        }

        internal ScriptStatement(string code, StatementType statementType)
        {
            Code = code;
            StatementType = statementType;
        }
    }
}
