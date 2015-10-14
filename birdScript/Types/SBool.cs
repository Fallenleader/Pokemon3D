using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SBool : SProtoObject
    {
        /// <summary>
        /// Converts the value to the script representation.
        /// </summary>
        internal static string ConvertToScriptString(bool value)
        {
            if (value)
                return LITERAL_BOOL_TRUE;
            else
                return LITERAL_BOOL_FALSE;
        }

        /// <summary>
        /// The value of this instance.
        /// </summary>
        internal bool Value { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SBool"/> class without setting a default value.
        /// </summary>
        internal SBool() { }

        private SBool(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates an instance of the <see cref="SBool"/> class and sets an initial value.
        /// </summary>
        internal static SBool Factory(bool value)
        {
            return new SBool(value);
        }

        internal override string ToScriptObject()
        {
            return ConvertToScriptString(Value);
        }

        internal override string ToScriptSource()
        {
            return ConvertToScriptString(Value);
        }

        internal override SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(ConvertToScriptString(Value));
        }

        internal override SBool ToBool(ScriptProcessor processor)
        {
            return processor.CreateBool(Value);
        }

        internal override SNumber ToNumber(ScriptProcessor processor)
        {
            if (Value)
                return processor.CreateNumber(1);
            else
                return processor.CreateNumber(0);
        }

        internal override string TypeOf()
        {
            return LITERAL_TYPE_BOOL;
        }

        internal override double SizeOf()
        {
            return 1;
        }
    }
}
