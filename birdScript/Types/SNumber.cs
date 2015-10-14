using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SNumber : SProtoObject
    {
        internal double Value { get; private set; }

        internal static string ConvertToScriptString(double value)
        {
            if (double.IsNaN(value))
                return LITERAL_NAN;
            else if (double.IsInfinity(value))
                return LITERAL_INFINITY;

            return value.ToString();
        }
    }
}
