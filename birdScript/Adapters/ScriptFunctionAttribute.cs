using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript.Adapters
{
    /// <summary>
    /// An attribute to add to methods that should get added as variables in adapted script objects.
    /// 
    /// The signature of the method has to conform to <see cref="DBuiltInMethod"/>.
    /// 
    /// This can also be applied to string fields, creating source functions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class ScriptFunctionAttribute : ScriptMemberAttribute
    { }
}
