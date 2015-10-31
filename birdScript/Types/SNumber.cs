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
        /// Tries to parse a numeric input to a <see cref="double"/>. Returns true for success.
        /// </summary>
        internal static bool TryParse(string input, out double result)
        {
            int numBase = 10;
            char baseChar = 'd'; //Standard -> decimal

            if (input.StartsWith("0b") ||
                input.StartsWith("0o") ||
                input.StartsWith("0x"))
            {
                baseChar = input[1];
                input = input.Remove(0, 2);
            }

            switch (baseChar)
            {
                case 'b': //b -> Binary
                    numBase = 2;
                    break;
                case 'o': //o -> Octal
                    numBase = 8;
                    break;
                case 'x': //x -> Hexadecimal
                    numBase = 16;
                    break;
            }

            if (baseChar != 'd')
            {
                int parseResult = 0;
                if (int.TryParse(input, out parseResult))
                {
                    result = Convert.ToInt32(input, numBase);
                    return true;
                }
                else
                {
                    result = 0;
                    return false;
                }
            }
            else
            {
                return double.TryParse(input, out result);
            }
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
            if (Prototype == null)
                return ToScriptSource();
            else
                return base.ToScriptObject();
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
            if (Prototype == null)
                return LITERAL_TYPE_NUMBER;
            else
                return base.TypeOf();
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
