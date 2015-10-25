using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace birdScript.Adapters
{
    public static class ScriptInAdapter
    {
        public static SObject Translate(ScriptProcessor processor, object objIn)
        {
            if (objIn.GetType() == typeof(SObject) || objIn.GetType().IsSubclassOf(typeof(SObject)))
            {
                // this is already an SObject, return it.
                return (SObject)objIn;
            }
            if (objIn == null)
            {
                return TranslateNull(processor);
            }
            else if (objIn is sbyte || objIn is byte || objIn is short || objIn is ushort || objIn is int || objIn is uint || objIn is long || objIn is ulong || objIn is float || objIn is double)
            {
                return TranslateNumber(processor, Convert.ToDouble(objIn));
            }
            else if (objIn is string || objIn is char)
            {
                return TranslateString(processor, objIn.ToString()); // ToString will return just the string for the string type, and a string from a char.
            }
            else if (objIn is bool)
            {
                return TranslateBool(processor, (bool)objIn);
            }
            else if (objIn.GetType().IsArray)
            {
                return TranslateArray(processor, (Array)objIn);
            }
            else if (objIn is DBuiltInMethod)
            {
                return TranslateFunction((DBuiltInMethod)objIn);
            }
            else
            {
                return TranslateObject(processor, objIn);
            }
        }

        private static SObject TranslateNull(ScriptProcessor processor)
        {
            return processor.Null;
        }

        private static SObject TranslateNumber(ScriptProcessor processor, double dblIn)
        {
            return processor.CreateNumber(dblIn);
        }

        private static SObject TranslateString(ScriptProcessor processor, string strIn)
        {
            return processor.CreateString(strIn);
        }

        private static SObject TranslateBool(ScriptProcessor processor, bool boolIn)
        {
            return processor.CreateBool(boolIn);
        }

        private static SObject TranslateFunction(DBuiltInMethod methodIn)
        {
            return new SFunction(methodIn);
        }

        private static SObject TranslateArray(ScriptProcessor processor, Array array)
        {
            List<SObject> elements = new List<SObject>();

            for (int i = 0; i < array.Length; i++)
                elements.Add(Translate(processor, array.GetValue(i)));
            
            return processor.Context.CreateInstance("Array", elements.ToArray());
        }

        private static SObject TranslateObject(ScriptProcessor processor, object objIn)
        {
            string typeName = objIn.GetType().Name;
            Prototype prototype = null;

            if (processor.Context.IsPrototype(typeName))
                prototype = processor.Context.GetPrototype(typeName);
            else
                prototype = TranslatePrototype(processor, objIn.GetType());
            
            var obj = prototype.CreateInstance(processor, null, false);

            FieldInfo[] fields = objIn.GetType()
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

                    obj.SetMember(identifier, Translate(processor, field.GetValue(objIn)));
                }
            }

            return obj;
        }

        internal static Prototype TranslatePrototype(ScriptProcessor processor, Type t)
        {
            var prototype = new Prototype(t.Name);

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

                    prototype.AddMember(processor, new PrototypeMember(identifier, processor.Undefined));
                }
            }

            MethodInfo[] methods = t
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ScriptVariableAttribute>(false);
                if (attr != null)
                {
                    string identifier = method.Name;
                    if (!string.IsNullOrEmpty(attr.VariableName))
                        identifier = attr.VariableName;

                    prototype.AddMember(processor, new PrototypeMember(identifier, processor.Undefined));
                }
            }

            prototype.MappedType = t;

            return prototype;
        }
    }
}
