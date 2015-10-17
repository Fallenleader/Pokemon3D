using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = "if(someshit) cw; else cw";

            var processor = new ScriptProcessor();
            var statements = StatementProcessor.GetStatements(processor, source);
        }
    }
}
