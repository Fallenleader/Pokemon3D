using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// The base script object.
    /// </summary>
    public abstract partial class SObject
    {
        /// <summary>
        /// Returns if the input object is null.
        /// </summary>
        protected internal static bool IsNull(SObject obj)
        {
            return ReferenceEquals(obj, null);
        }

        /// <summary>
        /// Unboxes an <see cref="SVariable"/> if the passed in object is one.
        /// </summary>
        internal static SObject Unbox(SObject obj)
        {
            while (obj is SVariable)
                obj = ((SVariable)obj).Data;

            return obj;
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
            return LITERAL_UNDEFINED;
        }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        internal virtual SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(LITERAL_UNDEFINED);
        }

        /// <summary>
        /// Returns the number representation of this object.
        /// </summary>
        internal virtual SNumber ToNumber(ScriptProcessor processor)
        {
            return processor.CreateNumber(double.NaN);
        }

        /// <summary>
        /// Returns the bool representation of this object.
        /// </summary>
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

        /// <summary>
        /// Returns a member variable of this object.
        /// </summary>
        internal virtual SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            return processor.Undefined;
        }

        /// <summary>
        /// Executes a member function of this object.
        /// </summary>
        internal virtual SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            return processor.Undefined;
        }

        /// <summary>
        /// Sets a member variable of this object to a new value.
        /// </summary>
        internal virtual void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value) { /* Empty */ }

        /// <summary>
        /// Returns if this object has a specific member.
        /// </summary>
        internal virtual bool HasMember(ScriptProcessor processor, string memberName)
        {
            return false;
        }

    }
}
