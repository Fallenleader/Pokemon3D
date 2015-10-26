using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Adapters
{
    public abstract class ScriptMemberAttribute : Attribute
    {
        /// <summary>
        /// If this is set, the value of this property will be used as the variable name.
        /// </summary>
        public string VariableName { get; set; }
    }
}
