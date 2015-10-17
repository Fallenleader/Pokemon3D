using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    public enum StatementType
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

    public class ScriptStatement
    {
        public string Code { get; }
        public StatementType StatementType { get; }

        internal SObject StatementResult;

        public ScriptStatement(string code)
        {
            Code = code.Trim();
            StatementType = StatementProcessor.GetStatementType(Code, true, false);
        }

        public ScriptStatement(string code, StatementType statementType)
        {
            Code = code;
            StatementType = statementType;
        }
    }
}
