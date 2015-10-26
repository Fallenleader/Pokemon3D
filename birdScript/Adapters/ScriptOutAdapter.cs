using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace birdScript.Adapters
{
    /// <summary>
    /// An adapter to convert script objects to regular .Net objects.
    /// </summary>
    public static class ScriptOutAdapter
    {
        /// <summary>
        /// Translates an <see cref="SObject"/> to a dynamic type.
        /// </summary>
        public static object Translate(SObject obj)
        {
            if (obj is SBool)
            {
                return ((SBool)obj).Value;
            }
            else if (obj is SString)
            {
                return ((SString)obj).Value;
            }
            else if (obj is SNumber)
            {
                return ((SNumber)obj).Value;
            }
            else if (obj is SArray)
            {
                return TranslateArray((SArray)obj);
            }
            else if (obj is SNull)
            {
                return null;
            }
            else if (obj is SUndefined)
            {
                return obj;
            }
            else if (obj is SFunction)
            {
                return TranslateFunction((SFunction)obj);
            }
            else if (obj is SProtoObject && ((SProtoObject)obj).Prototype.MappedType != null)
            {
                return Translate((SProtoObject)obj, ((SProtoObject)obj).Prototype.MappedType);
            }
            else if (obj is SProtoObject)
            {
                return TranslateDynamic((SProtoObject)obj);
            }
            else
            {
                return obj.ToScriptSource();
            }
        }

        private static object TranslateArray(SArray obj)
        {
            return obj.ArrayMembers.Select(x => Translate(x)).ToArray();
        }

        private static object TranslateDynamic(SProtoObject obj)
        {
            var returnObj = new ExpandoObject() as IDictionary<string, object>;

            foreach (var item in obj.Members)
            {
                string memberName = item.Key;
                // Do not translate back the prototype and super instances:
                if (memberName != SProtoObject.MEMBER_NAME_PROTOTYPE && 
                    memberName != SProtoObject.MEMBER_NAME_SUPER)
                {
                    SObject memberContent = SObject.Unbox(item.Value);
                    returnObj.Add(memberName, Translate(memberContent));
                }
            }

            return returnObj as dynamic;
        }

        private static object TranslateFunction(SFunction obj)
        {
            if (obj.Method != null)
                return obj.Method;
            else
                return obj.ToScriptSource();
        }

        /// <summary>
        /// Translates an <see cref="SObject"/> to a specific type.
        /// </summary>
        public static object Translate(SProtoObject obj, Type t)
        {
            var instance = Activator.CreateInstance(t);

            FieldInfo[] fields = t
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<ScriptVariableAttribute>(false);

                if (attr != null)
                {
                    string identifier = field.Name;
                    if (!string.IsNullOrEmpty(attr.VariableName))
                        identifier = attr.VariableName;

                    SObject setValue = SObject.Unbox(obj.Members[identifier]);
                    field.SetValue(instance, Translate(setValue));
                }
            }

            return instance;
        }
    }
}
