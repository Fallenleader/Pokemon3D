using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Adapters;

namespace birdScript.Types
{
    /// <summary>
    /// Represents a link to an API of another application imported via "using" statement.
    /// </summary>
    internal class SAPIUsing : SObject
    {
        /// <summary>
        /// The name of the API class.
        /// </summary>
        public string APIClass { get; }

        public SAPIUsing(string apiClass)
        {
            APIClass = apiClass;
        }

        internal override void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value)
        {
            if (processor.Context.HasCallback(CallbackType.SetMember))
            {
                var callback = (DSetMember)processor.Context.GetCallback(CallbackType.SetMember);
                Task task = Task.Factory.StartNew(() => callback(processor, accessor, isIndexer, value));
                task.Wait();
            }
            else
            {
                processor.ErrorHandler.ThrowError(ErrorType.APIError, ErrorHandler.MESSAGE_API_NOT_SUPPORTED);
            }
        }

        internal override SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            if (processor.Context.HasCallback(CallbackType.ExecuteMethod))
            {
                var callback = (DExecuteMethod)processor.Context.GetCallback(CallbackType.ExecuteMethod);
                Task<SObject> task = Task<SObject>.Factory.StartNew(() => callback(processor, methodName, parameters));
                task.Wait();

                return task.Result;
            }
            else
            {
                processor.ErrorHandler.ThrowError(ErrorType.APIError, ErrorHandler.MESSAGE_API_NOT_SUPPORTED);
                return processor.Undefined;
            }
        }

        internal override SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            if (processor.Context.HasCallback(CallbackType.GetMember))
            {
                var callback = (DGetMember)processor.Context.GetCallback(CallbackType.GetMember);
                Task<SObject> task = Task<SObject>.Factory.StartNew(() => callback(processor, accessor, isIndexer));
                task.Wait();

                return task.Result;
            }
            else
            {
                processor.ErrorHandler.ThrowError(ErrorType.APIError, ErrorHandler.MESSAGE_API_NOT_SUPPORTED);
                return processor.Undefined;
            }
        }

        internal override bool HasMember(ScriptProcessor processor, string memberName)
        {
            if (processor.Context.HasCallback(CallbackType.HasMember))
            {
                var callback = (DHasMember)processor.Context.GetCallback(CallbackType.HasMember);
                Task<bool> task = Task<bool>.Factory.StartNew(() => callback(processor, APIClass, memberName));
                task.Wait();

                return task.Result;
            }
            else
            {
                // If no API callback function is added to check for member existence, then we just assume that no member exists and return false.
                return false;
            }
        }

        internal override string ToScriptObject()
        {
            return APIClass;
        }

        internal override string ToScriptSource()
        {
            return APIClass;
        }

        internal override double SizeOf()
        {
            return APIClass.Length;
        }
    }
}
