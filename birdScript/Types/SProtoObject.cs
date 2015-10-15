using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// An object that is an instance of a <see cref="Prototypes.Prototype"/>.
    /// </summary>
    public class SProtoObject : SObject
    {
        internal Dictionary<string, SVariable> Members { get; private set; }

        internal SFunction IndexerGetFunction { get; set; }
        internal SFunction IndexerSetFunction { get; set; }

        internal void AddMember(SVariable member)
        {
            Members.Add(member.Identifier, member);
        }

        internal void AddMember(string idenfifier, SObject data)
        {
            Members.Add(idenfifier, new SVariable(idenfifier, data));
        }
    }
}
