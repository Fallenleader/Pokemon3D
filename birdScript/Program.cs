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
            bool exit = false;
            var processor = new ScriptProcessor();

            while (!exit)
            {
                Console.Write("< ");
                string input = Console.ReadLine();
                
                var result = processor.Run(input);

                if (result.TypeOf() == Types.SObject.LITERAL_TYPE_ERROR)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Types.SError error = (Types.SError)result;

                    Console.WriteLine("x " + ((Types.SString)error.Members["type"].Data).Value + ": " +
                                      ((Types.SString)error.Members["message"].Data).Value);

                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("> " + result.ToScriptSource());
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
