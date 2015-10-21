using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SError : SProtoObject
    {
        internal override string TypeOf()
        {
            return LITERAL_TYPE_ERROR;
        }
    }
}
