using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// An object that is an instance of a prototype.
    /// </summary>
    class SProtoObject : SObject
    {
        internal Dictionary<string, SVariable> Members { get; private set; }

        internal void AddMember(SVariable member)
        {
            Members.Add(member.Identifier, member);
        }
    }
}
