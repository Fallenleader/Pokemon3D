using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    public abstract partial class SObject
    {
        internal const string LITERAL_UNDEFINED = "undefined";
        internal const string LITERAL_OBJECT = "object";
        internal const string LITERAL_NULL = "null";

        internal const string LITERAL_PROTOTYPE = "[prototype]";
        internal const string LITERAL_OBJECT_STR = "[object Object]";

        internal const string LITERAL_TYPE_STRING = "string";
        internal const string LITERAL_TYPE_BOOL = "bool";
        internal const string LITERAL_TYPE_NUMBER = "number";
        internal const string LITERAL_TYPE_FUNCTION = "function";

        internal const string LITERAL_NAN = "NaN";
        internal const string LITERAL_INFINITY = "Infinity";

        internal const string LITERAL_BOOL_TRUE = "true";
        internal const string LITERAL_BOOL_FALSE = "false";
    }
}
