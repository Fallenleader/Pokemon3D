using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    /// <summary>
    /// Represents an array : a collection of objects. Not type-safe.
    /// </summary>
    internal class SArray : SProtoObject
    {
        /// <summary>
        /// The members of this array.
        /// </summary>
        public SObject[] ArrayMembers { get; private set; }

        /// <summary>
        /// Parses a string as an array.
        /// </summary>
        internal new static SObject Parse(ScriptProcessor processor, string exp)
        {
            // Format: [item1, item2, ... itemn]

            exp = exp.Remove(exp.Length - 1, 1).Remove(0, 1).Trim(); // Remove [ and ].

            List<SObject> elements = new List<SObject>();

            int elementStart = 0;
            int index = 0;
            int depth = 0;
            StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(exp, 0);
            string element;

            while (index < exp.Length)
            {
                char t = exp[index];
                escaper.CheckStartAt(index);

                if (!escaper.IsString)
                {
                    if (t == '{' || t == '[' || t == '(')
                    {
                        depth++;
                    }
                    else if (t == '}' || t == ']' || t == ')')
                    {
                        depth--;
                    }
                    else if (t == ',' && depth == 0)
                    {
                        element = exp.Substring(elementStart, index);
                        
                        if (string.IsNullOrWhiteSpace(element))
                            elements.Add(processor.Undefined);
                        else
                            elements.Add(processor.ExecuteStatement(new ScriptStatement(element)));

                        elementStart = index + 1;
                    }
                }

                index++;
            }

            element = exp.Substring(elementStart, index);

            if (string.IsNullOrWhiteSpace(element))
                elements.Add(processor.Undefined);
            else
                elements.Add(processor.ExecuteStatement(new ScriptStatement(element)));

            return processor.Context.CreateInstance("Array", elements.ToArray());
        }

        /// <summary>
        /// Updates the "length" property of this object.
        /// </summary>
        public void UpdateLength(ScriptProcessor processor)
        {
            //TODO: Add length to prototype
            Members["length"] = new SVariable("length", processor.CreateNumber(ArrayMembers.Length)) { IsReadOnly = true };
        }

        internal override string ToScriptSource()
        {
            StringBuilder source = new StringBuilder();

            foreach (var arrItem in ArrayMembers)
            {
                if (source.Length > 0)
                    source.Append(",");

                if (!ReferenceEquals(Unbox(arrItem), this))
                {
                    source.Append(arrItem.ToScriptSource());
                }
            }

            return source.ToString();
        }

        internal override double SizeOf()
        {
            return ArrayMembers.Length;
        }

        internal override SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(ToScriptSource());
        }
    }
}
