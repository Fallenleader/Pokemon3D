using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// Represents the "undefined" literal as object.
    /// </summary>
    class SUndefined : SObject
    {
        private SUndefined() { }
        
        /// <summary>
        /// Creates the undefined object.
        /// </summary>
        internal static SUndefined Create()
        {
            return new SUndefined();
        }

        internal override SBool ToBool(ScriptProcessor processor)
        {
            return processor.CreateBool(false);
        }

        internal override string TypeOf()
        {
            return "undefined";
        }
    }
}
