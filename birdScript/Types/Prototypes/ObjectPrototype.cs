using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types.Prototypes
{
    class ObjectPrototype : Prototype
    {
        public ObjectPrototype() : base("Object") { }

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
                    return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_REFERENCE_NO_PROTOTYPE, protoParam.TypeOf());
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
                return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_REFERENCE_NO_PROTOTYPE, LITERAL_UNDEFINED);
            }
        }

        [BuiltInMethod(IsStatic = true)]
        public static SObject addMember(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            // Parameter #1: (String)Name of the new member
            // [Parameter #2: Default value of the new member ] / Undefined
            // [Parameter #3: Signature config of the new member] / instance member, no special settings

            if (parameters.Length == 0)
                return processor.Undefined;

            Prototype prototype;

            if (IsPrototype(instance.GetType()))
            {
                prototype = (Prototype)instance;
            }
            else
            {
                // The instance will be a prototype instance, so get its prototype from there:
                var protoObj = (SProtoObject)instance;
                prototype = protoObj.Prototype;
            }

            string memberName;
            if (parameters[0] is SString)
                memberName = ((SString)parameters[0]).Value;
            else
                memberName = parameters[0].ToString(processor).Value;

            SObject defaultValue = processor.Undefined;

            if (parameters.Length > 1)
            {
                defaultValue = parameters[1];
            }

            //if (parameters.Length > 2)
            //{

            //}

            if (!ScriptProcessor.IsValidIdentifier(memberName))
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            prototype.AddMember(processor, new PrototypeMember(memberName, defaultValue));

            return processor.Undefined;
        }
    }
}
