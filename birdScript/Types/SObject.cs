using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// The delegate for hardcoded methods.
    /// </summary>
    /// <param name="instance">The calling instance.</param>
    /// <param name="processor">The script processor this call originates from.</param>
    /// <param name="This">The contextual "this" object.</param>
    /// <param name="parameters">Parameters for this method call.</param>
    public delegate SObject DBuiltInMethod(SObject instance, ScriptProcessor processor, SObject This, SObject[] parameters);

    /// <summary>
    /// The base script object.
    /// </summary>
    public abstract partial class SObject
    {
        /// <summary>
        /// Returns if the input object is null.
        /// </summary>
        protected static bool IsNull(SObject obj)
        {
            return ReferenceEquals(obj, null);
        }

        /// <summary>
        /// Returns a representation of the current object that the script processor works with.
        /// </summary>
        internal virtual string ToScriptObject()
        {
            return LITERAL_UNDEFINED;
        }

        /// <summary>
        /// Returns a representation of the current object that exposes the source of it.
        /// </summary>
        internal virtual string ToScriptSource()
        {
            return ToScriptObject();
        }

        internal virtual SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(LITERAL_UNDEFINED);
        }

        internal virtual SNumber ToNumber(ScriptProcessor processor)
        {
            return processor.CreateNumber(double.NaN);
        }

        internal virtual SBool ToBool(ScriptProcessor processor)
        {
            return processor.CreateBool(true);
        }

        internal virtual string TypeOf()
        {
            return LITERAL_OBJECT;
        }

        internal virtual double SizeOf()
        {
            return 0;
        }

        internal virtual SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            return processor.Undefined;
        }

        internal virtual SObject ExecuteMethod(string methodName, SObject caller, ScriptProcessor processor, SObject This, SObject[] parameters)
        {
            return processor.Undefined;
        }

        internal virtual void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value) { /* Empty */ }

        internal virtual bool HasMember(string memberName)
        {
            return false;
        }
    }
}
