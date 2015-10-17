using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// Represents the "null" literal as object.
    /// </summary>
    internal class SNull : SObject
    {
        private SNull() { }

        /// <summary>
        /// Creates the null object.
        /// </summary>
        internal static SNull Factory()
        {
            return new SNull();
        }

        internal override string ToScriptObject()
        {
            return LITERAL_NULL;
        }

        internal override string ToScriptSource()
        {
            return LITERAL_NULL;
        }

        internal override SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(LITERAL_NULL);
        }

        internal override SBool ToBool(ScriptProcessor processor)
        {
            return processor.CreateBool(false);
        }

        internal override SNumber ToNumber(ScriptProcessor processor)
        {
            return processor.CreateNumber(0);
        }

        internal override string TypeOf()
        {
            return LITERAL_NULL;
        }

        internal override double SizeOf()
        {
            return 0;
        }
    }
}
