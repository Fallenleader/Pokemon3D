using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SBool : SCustomObject
    {
        internal bool Value { get; private set; }

        internal static string ConvertToScriptString(bool value)
        {
            if (value)
                return LITERAL_BOOL_TRUE;
            else
                return LITERAL_BOOL_FALSE;
        }
    }
}
