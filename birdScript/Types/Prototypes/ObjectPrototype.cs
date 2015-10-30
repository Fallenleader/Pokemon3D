using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types.Prototypes
{
    class ObjectPrototype : Prototype
    {
        public ObjectPrototype() : base("Object")
        {

        }

        [BuiltInMethod]
        public static SObject toString(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            return processor.CreateString(LITERAL_OBJECT_STR);
        }

        [BuiltInMethod(IsStatic = true)]
        public static SObject create(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            Prototype prototype = null;

            if (parameters.Length > 0)
            {
                var protoParam = parameters[0];
                if (protoParam.TypeOf() == LITERAL_TYPE_STRING)
                {
                    prototype = processor.Context.GetPrototype(((SString)protoParam).Value);
                }
                else if (IsPrototype(protoParam.GetType()))
                {
                    prototype = (Prototype)protoParam;
                }
                else
                {
                    return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_REFERENCE_NO_PROTOTYPE,  protoParam.TypeOf() );
                }
            }

            if (prototype != null)
            {
                var instParams = new SObject[parameters.Length - 1];
                Array.Copy(parameters, 1, instParams, 0, parameters.Length - 1);

                return processor.Context.CreateInstance(prototype, instParams);
            }
            else
            {
                return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_REFERENCE_NO_PROTOTYPE,  LITERAL_UNDEFINED );
            }
        }
    }
}
