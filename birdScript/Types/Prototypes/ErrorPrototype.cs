using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types.Prototypes
{
    internal class ErrorPrototype : Prototype
    {
        internal const string MEMBER_NAME_MESSAGE = "message";
        internal const string MEMBER_NAME_TYPE = "type";
        internal const string MEMBER_NAME_LINE = "line";

        public ErrorPrototype(ScriptProcessor processor) : base("Error")
        {
            Constructor = new PrototypeMember(CLASS_METHOD_CTOR, new SFunction(constructor));

            AddMember(processor, new PrototypeMember(MEMBER_NAME_MESSAGE, processor.Undefined));
            AddMember(processor, new PrototypeMember(MEMBER_NAME_TYPE, processor.Undefined));
            AddMember(processor, new PrototypeMember(MEMBER_NAME_LINE, processor.Undefined));
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

                obj.Members[MEMBER_NAME_MESSAGE].Data = message;
            }

            if (parameters.Length > 1)
            {
                SString errorType;

                if (parameters[1] is SString)
                    errorType = (SString)parameters[1];
                else
                    errorType = parameters[1].ToString(processor);

                obj.Members[MEMBER_NAME_TYPE].Data = errorType;
            }

            if (parameters.Length > 2)
            {
                SNumber errorLine;
                if (parameters[2] is SNumber)
                    errorLine = (SNumber)parameters[2];
                else
                    errorLine = parameters[2].ToNumber(processor);

                obj.Members[MEMBER_NAME_LINE].Data = errorLine;
            }
            else
            {
                obj.Members[MEMBER_NAME_LINE].Data = processor.CreateNumber(-1);
            }

            return obj;
        }
    }
}
