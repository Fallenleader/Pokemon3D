using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SString : SProtoObject
    {
        internal const string STRING_LENGTH_PROPERTY_NAME = "length";

        private const string STRING_NORMAL_FORMAT = "\"{0}\"";
        private const string STRING_UNESCAPED_FORMAT = "@\"{0}\"";

        private static string Unescape(string val)
        {
            if (val.Contains("\\"))
            {
                int searchOffset = 0;

                while (val.IndexOf("\\", searchOffset) > -1)
                {
                    int cIndex = val.IndexOf("\\");

                    if (cIndex < val.Length - 1) //When the \ is not the last character:
                    {
                        char escapeSequenceChar = val[cIndex + 1];
                        string insert;

                        switch (escapeSequenceChar)
                        {
                            case '0':
                                insert = ((char)0).ToString();
                                break;
                            case '\'':
                                insert = "\'";
                                break;
                            case '\"':
                                insert = "\"";
                                break;
                            case '\\':
                                insert = "\\";
                                break;
                            case 'n':
                                insert = "\n";
                                break;
                            case 'r':
                                insert = "\r";
                                break;
                            case 'v':
                                insert = "\v";
                                break;
                            case 't':
                                insert = "\t";
                                break;
                            case 'b':
                                insert = "\b";
                                break;
                            case 'f':
                                insert = "\f";
                                break;

                            default:
                                insert = "";
                                break;
                        }

                        val = val.Remove(cIndex) + insert + val.Remove(0, cIndex + 2);
                    }

                    searchOffset = cIndex + 1;
                }
            }
            return val;
        }

        /// <summary>
        /// The value of this instance.
        /// </summary>
        internal string Value { get; private set; }

        /// <summary>
        /// If this instance has escaped characters or not. If not, the script representation will have an "@" in front of the " or '.
        /// </summary>
        internal bool Escaped { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SString"/> class without setting a default value.
        /// </summary>
        internal SString() { }

        private SString(ScriptProcessor processor, string value, bool escaped)
        {
            Escaped = escaped;

            if (escaped)
                SetValue(processor, Unescape(value));
            else
                SetValue(processor, value);
        }

        /// <summary>
        /// Creates an instance of the <see cref="SString"/> class and sets an initial value.
        /// </summary>
        internal static SString Factory(ScriptProcessor processor, string value, bool escaped)
        {
            return new SString(processor, value, escaped);
        }

        /// <summary>
        /// Sets the value and updates the length property.
        /// </summary>
        internal void SetValue(ScriptProcessor processor, string value)
        {
            Value = value;

            //var length = processor.CreateNumber(value.Length);

            //TODO: add length to prototype.
            //Members[STRING_LENGTH_PROPERTY_NAME].ForceSetData(length);
        }
        
        internal override string ToScriptObject()
        {
            if (Escaped)
                return string.Format(STRING_NORMAL_FORMAT, Value);
            else
                return string.Format(STRING_UNESCAPED_FORMAT, Value);
        }

        internal override string ToScriptSource()
        {
            return ToScriptObject();
        }

        internal override SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(Value, Escaped);
        }

        internal override SBool ToBool(ScriptProcessor processor)
        {
            return processor.CreateBool(Value != "");
        }

        internal override SNumber ToNumber(ScriptProcessor processor)
        {
            if (Value.Trim() == "")
            {
                return processor.CreateNumber(0);
            }
            else
            {
                double dblResult = 0;
                if (double.TryParse(Value, out dblResult))
                {
                    return processor.CreateNumber(dblResult);
                }
                else
                {
                    return processor.CreateNumber(double.NaN);
                }
            }
        }

        internal override string TypeOf()
        {
            return LITERAL_TYPE_STRING;
        }

        internal override double SizeOf()
        {
            return Value.Length;
        }
    }
}
