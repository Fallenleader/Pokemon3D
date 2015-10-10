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

        internal const string LITERAL_TYPE_STRING = "string";
        internal const string LITERAL_TYPE_BOOL = "bool";
        internal const string LITERAL_TYPE_NUMBER = "number";
    }
}
