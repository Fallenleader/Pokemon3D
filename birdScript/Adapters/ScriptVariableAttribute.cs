using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Adapters
{
    /// <summary>
    /// An attribute to add to fields that should get added as variables in adapted script objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ScriptVariableAttribute : ScriptMemberAttribute
    { }
}
