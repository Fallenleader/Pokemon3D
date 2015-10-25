using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;

namespace birdScript.Adapters
{
    /// <summary>
    /// Enables <see cref="ScriptContext"/> manipulations for Variables and Prototypes.
    /// </summary>
    public static class ScriptContextManipulator
    {
        /// <summary>
        /// Adds a new variable or overwrites one with the same name.
        /// </summary>
        public static void AddVariable(ScriptProcessor processor, string identifier, object data)
        {
            AddVariable(processor, identifier, ScriptInAdapter.Translate(processor, data));
        }

        /// <summary>
        /// Adds a new variable or overwrites one with the same name.
        /// </summary>
        public static void AddVariable(ScriptProcessor processor, string identifier, SObject data)
        {
            processor.Context.AddVariable(identifier, data);
        }

        /// <summary>
        /// Creates a <see cref="Prototype"/> from a .net <see cref="Type"/> and adds it to the context.
        /// </summary>
        public static void AddPrototype(ScriptProcessor processor, Type t)
        {
            processor.Context.AddPrototype(ScriptInAdapter.TranslatePrototype(processor, t));
        }

        /// <summary>
        /// Returns the content of a variable, or Undefined, if the variable does not exist.
        /// </summary>
        public static SObject GetVariable(ScriptProcessor processor, string identifier)
        {
            if (processor.Context.IsVariable(identifier))
                return processor.Context.GetVariable(identifier).Data;
            else
                return processor.Undefined;
        }

        /// <summary>
        /// Returns the translated content of a variable.
        /// </summary>
        public static object GetVariableTranslated(ScriptProcessor processor, string identifier)
        {
            return ScriptOutAdapter.Translate(GetVariable(processor, identifier));
        }

        /// <summary>
        /// Returns if the context has a specific variable.
        /// </summary>
        public static bool HasVariable(ScriptProcessor processor, string identifier)
        {
            return processor.Context.IsVariable(identifier);
        }
    }
}
