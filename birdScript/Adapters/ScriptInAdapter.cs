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

            // Set the field values of the current instance:

            FieldInfo[] fields = objIn.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(false);

                foreach (var attr in attributes)
                {
                    if (attr.GetType() == typeof(ScriptVariableAttribute))
                    {
                        var memberAttr = (ScriptMemberAttribute)attr;

                        string identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var fieldContent = field.GetValue(objIn);

                        obj.SetMember(identifier, Translate(processor, fieldContent));
                    }
                    else if (attr.GetType() == typeof(ScriptFunctionAttribute))
                    {
                        // When it's a field and a function, we have the source code of the function as value of the field.
                        // Example: public string MyFunction = "function() { console.log('Hello World'); }";

                        var memberAttr = (ScriptMemberAttribute)attr;

                        string identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        string functionCode = field.GetValue(objIn).ToString();

                        obj.SetMember(identifier, new SFunction(processor, functionCode));
                    }
                }
            }
            
            return obj;
        }

        internal static Prototype TranslatePrototype(ScriptProcessor processor, Type t)
        {
            var prototype = new Prototype(t.Name);

            var typeInstance = Activator.CreateInstance(t);

            FieldInfo[] fields = t
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(false);

                foreach (var attr in attributes)
                {
                    if (attr.GetType() == typeof(ScriptVariableAttribute))
                    {
                        var memberAttr = (ScriptMemberAttribute)attr;
                        string identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var fieldContent = field.GetValue(typeInstance);

                        if (fieldContent == null)
                            prototype.AddMember(processor, new PrototypeMember(identifier, processor.Undefined));
                        else
                            prototype.AddMember(processor, new PrototypeMember(identifier, Translate(processor, fieldContent)));
                    }
                    else if (attr.GetType() == typeof(ScriptFunctionAttribute))
                    {
                        var memberAttr = (ScriptMemberAttribute)attr;
                        string identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var fieldContent = field.GetValue(typeInstance);

                        if (fieldContent == null)
                            prototype.AddMember(processor, new PrototypeMember(identifier, processor.Undefined));
                        else
                            prototype.AddMember(processor, new PrototypeMember(identifier, new SFunction(processor, fieldContent.ToString())));
                    }
                }
            }

            MethodInfo[] methods = t
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ScriptFunctionAttribute>(false);
                if (attr != null)
                {
                    string identifier = method.Name;
                    if (!string.IsNullOrEmpty(attr.VariableName))
                        identifier = attr.VariableName;

                    var methodDelegate = (DBuiltInMethod)Delegate.CreateDelegate(typeof(DBuiltInMethod), method);

                    prototype.AddMember(processor, new PrototypeMember(identifier, new SFunction(methodDelegate)));
                }
            }

            prototype.MappedType = t;

            return prototype;
        }
    }
}
