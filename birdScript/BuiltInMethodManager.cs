using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;
using System.Reflection;

namespace birdScript
{
    /// <summary>
    /// The delegate for hardcoded methods.
    /// </summary>
    /// <param name="instance">The calling instance.</param>
    /// <param name="processor">The script processor this call originates from.</param>
    /// <param name="This">The contextual "this" object.</param>
    /// <param name="parameters">Parameters for this method call.</param>
    public delegate SObject DBuiltInMethod(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters);

    /// <summary>
    /// Searches for and creates built in method delegates.
    /// </summary>
    internal static class BuiltInMethodManager
    {
        /// <summary>
        /// Returns a list of methods with: Methodname, Method Attribute and Method Delegate.
        /// </summary>
        internal static List<Tuple<string, BuiltInMethodAttribute, DBuiltInMethod>> GetMethods(Type t)
        {
            var list = new List<Tuple<string, BuiltInMethodAttribute, DBuiltInMethod>>();
            bool isPrototype = Prototype.IsPrototype(t);

            var methods = t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (MethodInfo method in methods)
            {
                var attribute = method.GetCustomAttribute<BuiltInMethodAttribute>(false);

                // Only proceed if the method has the correct attribute
                if (attribute != null && (!attribute.IsStatic || isPrototype))
                {
                    string usedMethodName = method.Name;

                    if (!string.IsNullOrEmpty(attribute.MethodName))
                        usedMethodName = attribute.MethodName;
                    
                    list.Add(new Tuple<string, BuiltInMethodAttribute, DBuiltInMethod>(
                        usedMethodName,
                        attribute,
                        (DBuiltInMethod)Delegate.CreateDelegate(typeof(DBuiltInMethod), method)
                        ));
                }
            }

            return list;
        }
    }

    /// <summary>
    /// An attribute added to methods to mark them as built in methods of <see cref="Prototype"/>s.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class BuiltInMethodAttribute : Attribute
    {
        /// <summary>
        /// If this is set, the value of this property will be used as identifier instead of the method name of the method of this attribute.
        /// </summary>
        public string MethodName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsIndexerGet { get; set; }

        public bool IsIndexerSet { get; set; }

        public FunctionUsageType FunctionType { get; set; }
    }
}
