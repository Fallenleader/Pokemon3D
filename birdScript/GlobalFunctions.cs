using System.Collections.Generic;
using System.Linq;
using birdScript.Types;
using birdScript.Types.Prototypes;

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

        /// <summary>
        /// Mirrors the eval() function of JavaScript.
        /// </summary>
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

        [BuiltInMethod(MethodName = "sizeof")]
        public static SObject SizeOf(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length == 0)
                return processor.Undefined;

            return processor.CreateNumber(parameters[0].SizeOf());
        }

        [BuiltInMethod(MethodName = "typeof")]
        public static SObject TypeOf(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length == 0)
                return processor.Undefined;

            return processor.CreateString(parameters[0].TypeOf());
        }

        /// <summary>
        /// This is not like the C# nameof operator - it rather returns the actual typed name of the object, instead of just "object".
        /// </summary>
        [BuiltInMethod(MethodName = "nameof")]
        public static SObject NameOf(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length == 0)
                return processor.Undefined;

            if (parameters[0] is SProtoObject)
            {
                var protoObj = (SProtoObject)parameters[0];
                if (protoObj.IsProtoInstance)
                    return processor.CreateString(protoObj.Prototype.Name);
                else
                    return processor.CreateString(protoObj.TypeOf());
            }
            else
            {
                return processor.CreateString(parameters[0].TypeOf());
            }
        }

        /// <summary>
        /// Converts a primitive object (SString, SNumber, SBool) into their prototype counterparts.
        /// </summary>
        [BuiltInMethod(MethodName = "toComplex")]
        public static SObject ToComplex(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length == 0)
                return processor.Undefined;

            if (parameters[0] is SString)
                return processor.Context.CreateInstance("String", new SObject[] { parameters[0] });
            else if (parameters[0] is SNumber)
                return processor.Context.CreateInstance("Number", new SObject[] { parameters[0] });
            else if (parameters[0] is SBool)
                return processor.Context.CreateInstance("Boolean", new SObject[] { parameters[0] });
            else
                return processor.Undefined;
        }

        /// <summary>
        /// Converts a complex object (SString, SNumber, SBool) back into their primitive counterparts.
        /// </summary>
        [BuiltInMethod(MethodName = "toPrimitive")]
        public static SObject ToPrimitive(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length == 0)
                return processor.Undefined;

            if (parameters[0] is SString)
                return processor.CreateString(((SString)parameters[0]).Value);
            else if (parameters[0] is SNumber)
                return processor.CreateNumber(((SNumber)parameters[0]).Value);
            else if (parameters[0] is SBool)
                return processor.CreateBool(((SBool)parameters[0]).Value);
            else
                return processor.Undefined;
        }
    }
}
