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
            string source = System.IO.File.ReadAllText(@"C:\Users\Nils\Desktop\script.js");

            var processor = new ScriptProcessor();
            var test = processor.Run(source);

            Console.WriteLine(test.ToScriptSource());
            Console.ReadKey();
        }
    }
}
