using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Adapters
{
    /// <summary>
    /// An attribute to add to methods that should get added as variables in adapted script objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class ScriptFunctionAttribute : Attribute
    {
        /// <summary>
        /// If this is set, the value of this property will be used as the variable name.
        /// </summary>
        public string VariableName { get; set; }
    }
}
