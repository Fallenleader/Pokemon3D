using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SNumber : SProtoObject
    {
        /// <summary>
        /// Converts the value to the script representation.
        /// </summary>
        internal static string ConvertToScriptString(double value)
        {
            if (double.IsNaN(value))
                return LITERAL_NAN;
            else if (double.IsInfinity(value))
                return LITERAL_INFINITY;

            return value.ToString();
        }

        /// <summary>
        /// The value of this instance.
        /// </summary>
        internal double Value { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SNumber"/> class without setting a default value.
        /// </summary>
        internal SNumber() { }

        private SNumber(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates an instance of the <see cref="SNumber"/> class and sets an initial value.
        /// </summary>
        internal static SNumber Factory(double value)
        {
            return new SNumber(value);   
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
            return processor.CreateBool(Value != 0);
        }

        internal override SNumber ToNumber(ScriptProcessor processor)
        {
            return processor.CreateNumber(Value);
        }

        internal override string TypeOf()
        {
            return LITERAL_TYPE_NUMBER;
        }

        internal override double SizeOf()
        {
            if (double.IsNaN(Value))
                return 0;
            else if (double.IsInfinity(Value))
                return -1;
            else
                return Value.ToString().Length;
        }
    }
}
