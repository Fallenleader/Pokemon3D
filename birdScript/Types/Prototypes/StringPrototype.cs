using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types.Prototypes
{
    class StringPrototype : Prototype
    {
        public StringPrototype() : base("String")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(constructor));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SString();
        }

        private static SObject constructor(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            SString obj = (SString)instance;

            if (parameters[0] is SString)
            {
                obj.SetValue(processor, ((SString)parameters[0]).Value);
                obj.Escaped = ((SString)parameters[0]).Escaped;
            }
            else
                obj.SetValue(processor, parameters[0].ToString(processor).Value);

            return obj;
        }
    }
}
