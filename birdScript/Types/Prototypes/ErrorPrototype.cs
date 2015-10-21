using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types.Prototypes
{
    class ErrorPrototype : Prototype
    {
        public ErrorPrototype(ScriptProcessor processor) : base("Error")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(constructor));

            AddMember(processor, new PrototypeMember("message", processor.Undefined));
            AddMember(processor, new PrototypeMember("type", processor.Undefined));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SError();
        }

        private static SObject constructor(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            SError obj = (SError)instance;

            if (parameters.Length > 0)
            {
                SString message;

                if (parameters[0] is SString)
                    message = (SString)parameters[0];
                else
                    message = parameters[0].ToString(processor);

                obj.Members["message"].Data = message;
            }

            if (parameters.Length > 1)
            {
                SString errorType;

                if (parameters[1] is SString)
                    errorType = (SString)parameters[1];
                else
                    errorType = parameters[1].ToString(processor);

                obj.Members["type"].Data = errorType;
            }

            return obj;
        }
    }
}
