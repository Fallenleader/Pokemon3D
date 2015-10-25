using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types.Prototypes
{
    class ArrayPrototype : Prototype
    {
        public ArrayPrototype() : base("Array")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(constructor));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SArray();
        }

        private static SObject constructor(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            SArray arr = (SArray)instance;
            if (parameters.Length == 1 && parameters[0].TypeOf() == LITERAL_TYPE_NUMBER)
            {
                int length = (int)((SNumber)parameters[0]).Value;

                if (length >= 0)
                {
                    arr.ArrayMembers = new SObject[length];
                    for (int i = 0; i < length; i++)
                    {
                        arr.ArrayMembers[i] = processor.Undefined;
                    }
                }
                else
                {
                    arr.ArrayMembers = new SObject[0];
                }
                return arr;
            }
            else
            {
                arr.ArrayMembers = parameters;
                arr.UpdateLength(processor);
                return arr;
            }
        }

        [BuiltInMethod(IsIndexerSet = true)]
        public static SObject indexerSet(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            SArray arr = (SArray)instance;

            if (parameters.Length >= 2)
            {
                int accessor = (int)parameters[0].ToNumber(processor).Value;

                if (accessor >= 0 && accessor < arr.ArrayMembers.Length)
                {
                    arr.ArrayMembers[accessor] = parameters[1];
                }
            }

            return processor.Undefined;
        }

        [BuiltInMethod(IsIndexerGet = true)]
        public static SObject indexerGet(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            SArray arr = (SArray)instance;

            if (parameters.Length >= 1)
            {
                int accessor = (int)parameters[0].ToNumber(processor).Value;

                if (accessor >= 0 && accessor < arr.ArrayMembers.Length)
                {
                    return arr.ArrayMembers[accessor];
                }
            }

            return processor.Undefined;
        }
    }
}
