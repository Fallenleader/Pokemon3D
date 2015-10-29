using System.Collections.Generic;
using System.Linq;
using birdScript.Types;

namespace birdScript
{
    /// <summary>
    /// A class containing all globally accessible script functions.
    /// </summary>
    static class GlobalFunctions
    {
        /// <summary>
        /// Creates an array of script functions built from the static BuiltInMethods of this class.
        /// </summary>
        internal static List<SVariable> GetFunctions()
        {
            List<SVariable> functions = new List<SVariable>();

            foreach (var methodDesc in BuiltInMethodManager.GetMethods(typeof(GlobalFunctions)))
                functions.Add(new SVariable(methodDesc.Item1, new SFunction(methodDesc.Item3), true));

            return functions;
        }

        [BuiltInMethod(MethodName = "eval")]
        public static SObject Eval(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length == 0)
                return processor.Undefined;

            string code;
            if (parameters[0] is SString)
                code = ((SString)parameters[0]).Value;
            else
                code = parameters[0].ToString(processor).Value;

            var evalProcessor = new ScriptProcessor(processor.Context);
            return evalProcessor.Run(code);
        }
    }
}
