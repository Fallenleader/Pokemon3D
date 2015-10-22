using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types.Prototypes;
using System.Text.RegularExpressions;

namespace birdScript.Types
{
    /// <summary>
    /// An object that is an instance of a <see cref="Prototypes.Prototype"/>.
    /// </summary>
    public class SProtoObject : SObject
    {
        internal protected const string MEMBER_NAME_PROTOTYPE = "prototype";
        internal protected const string MEMBER_NAME_SUPER = "super";

        internal Dictionary<string, SVariable> Members { get; } = new Dictionary<string, SVariable>();

        internal SFunction IndexerGetFunction { get; set; }
        internal SFunction IndexerSetFunction { get; set; }

        internal void AddMember(SVariable member)
        {
            Members.Add(member.Identifier, member);
        }

        internal void AddMember(string idenfifier, SObject data)
        {
            Members.Add(idenfifier, new SVariable(idenfifier, data));
        }

        internal SProtoObject SuperClass
        {
            get
            {
                if (Members.ContainsKey(MEMBER_NAME_SUPER))
                {
                    return (SProtoObject)Members[MEMBER_NAME_SUPER].Data;
                }
                return null;
            }
        }

        internal Prototype Prototype
        {
            get
            {
                if (Members.ContainsKey(MEMBER_NAME_PROTOTYPE))
                {
                    return (Prototype)Members[MEMBER_NAME_PROTOTYPE].Data;
                }
                return null;
            }
        }

        internal override bool HasMember(ScriptProcessor processor, string memberName)
        {
            if (Members.ContainsKey(memberName))
            {
                return true;
            }
            else if (Prototype != null && Prototype.HasMember(processor, memberName))
            {
                return true;
            }
            else if (SuperClass != null)
            {
                return SuperClass.HasMember(processor, memberName);
            }
            return false;
        }

        internal override SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            if (isIndexer && accessor.TypeOf() == LITERAL_TYPE_NUMBER)
            {
                if (IndexerGetFunction != null)
                {
                    return IndexerGetFunction.Call(processor, this, this, new SObject[] { accessor });
                }
                else
                {
                    return processor.Undefined;
                }
            }

            string memberName;
            if (accessor is SString)
                memberName = ((SString)accessor).Value;
            else
                memberName = accessor.ToString(processor).Value;

            if (Members.ContainsKey(memberName))
            {
                return Members[memberName];
            }
            else if (Prototype != null && Prototype.HasMember(processor, memberName))
            {
                return Prototype.GetMember(processor, accessor, isIndexer);
            }
            else if (SuperClass != null)
            {
                return SuperClass.GetMember(processor, accessor, isIndexer);
            }

            return processor.Undefined;
        }

        internal override void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value)
        {
            if (isIndexer && accessor.TypeOf() == LITERAL_TYPE_NUMBER && IndexerSetFunction != null)
            {
                IndexerSetFunction.Call(processor, this, this, new SObject[] { accessor, value });
            }
            else
            {
                string memberName;
                if (accessor is SString)
                    memberName = ((SString)accessor).Value;
                else
                    memberName = accessor.ToString(processor).Value;

                if (Members.ContainsKey(memberName))
                {
                    Members[memberName].Data = value;
                }
                else if (Prototype != null && Prototype.HasMember(processor, memberName) && !Prototype.IsStaticMember(memberName))
                {
                    // This is the case when new members got added to the prototype, and we haven't copied them over to the instance yet.
                    // So we do that now, and then set the value of that member:
                    AddMember(memberName, value);
                }
                else if (SuperClass != null)
                {
                    SuperClass.SetMember(processor, accessor, isIndexer, value);
                }
            }
        }

        internal override SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            if (Members.ContainsKey(methodName))
            {
                if (Members[methodName].Data.TypeOf() == LITERAL_TYPE_FUNCTION)
                {
                    return ((SFunction)Members[methodName].Data).Call(processor, caller, this, parameters);
                }
                else
                {
                    return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_TYPE_NOT_A_FUNCTION, new object[] { methodName });
                }
            }
            else if (Prototype != null && Prototype.HasMember(processor, methodName))
            {
                return Prototype.ExecuteMethod(processor, methodName, caller, This, parameters);
            }
            else if (SuperClass != null)
            {
                return SuperClass.ExecuteMethod(processor, methodName, caller, This, parameters);
            }
            else
            {
                return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_TYPE_NOT_A_FUNCTION, new object[] { methodName });
            }
        }

        internal override string ToScriptObject()
        {
            return "$" + ObjectBuffer.GetObjectId(this).ToString();
        }

        private const string FORMAT_SOURCE_MEMBER = "{0}:{1}";

        internal override string ToScriptSource()
        {
            StringBuilder source = new StringBuilder();

            foreach (var member in Members)
            {
                if (member.Key != MEMBER_NAME_PROTOTYPE && member.Key != MEMBER_NAME_SUPER) // Do not include super class or prototype in the string representation
                {
                    if (source.Length > 0)
                        source.Append(",");

                    var data = Unbox(member.Value);

                    if (ReferenceEquals(data, this))
                    {
                        // Avoid infinite recursion:
                        source.Append(string.Format(FORMAT_SOURCE_MEMBER, member.Key, "{}"));
                    }
                    else
                    {
                        source.Append(string.Format(FORMAT_SOURCE_MEMBER, member.Key, data.ToScriptSource()));
                    }
                }
            }

            source.Insert(0, "{");
            source.Append("}");

            return source.ToString();
        }

        internal override double SizeOf()
        {
            return Members.Count;
        }

        internal override SString ToString(ScriptProcessor processor)
        {
            return processor.CreateString(LITERAL_OBJECT_STR);
        }

        /// <summary>
        /// Parses an anonymous object.
        /// </summary>
        internal static SProtoObject Parse(ScriptProcessor processor, string source)
        {
            Prototype prototype = new Prototype(LITERAL_OBJECT);

            source = source.Trim();

            // Format: { member1 : content1, member2 : content2 }

            // Remove "{" and "}":
            source = source.Remove(source.Length - 1, 1).Remove(0, 1).Trim();

            if (source.Length == 0)
                return prototype.CreateInstance(processor, null, false);

            int index = 0;
            string identifier = "";
            string content = "";
            SObject contentObj = null;

            while (index < source.Length)
            {
                int nextSeperatorIndex = source.IndexOf(",", index);
                if (nextSeperatorIndex == -1)
                {
                    nextSeperatorIndex = source.Length;
                }
                else
                {
                    //Let's find the correct ",":

                    nextSeperatorIndex = index;

                    int depth = 0;
                    StringEscapeHelper escaper = new LeftToRightStringEscapeHelper(source, nextSeperatorIndex, true);
                    bool foundSeperator = false;

                    while (!foundSeperator && nextSeperatorIndex < source.Length)
                    {
                        char t = source[nextSeperatorIndex];

                        escaper.CheckStartAt(nextSeperatorIndex);

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
                                foundSeperator = true;
                                nextSeperatorIndex--; // it adds one to the index at the end of the while loop, but we need the index where we stopped searching, so we subtract 1 here to accommodate for that.
                            }
                        }

                        nextSeperatorIndex++;
                    }
                }

                string member = source.Substring(index, nextSeperatorIndex - index);

                if (member.Contains(":"))
                {
                    identifier = member.Remove(member.IndexOf(":")).Trim();
                    content = member.Remove(0, member.IndexOf(":") + 1);

                    contentObj = processor.ExecuteStatement(new ScriptStatement(content));
                }
                else
                {
                    identifier = member.Trim();
                    contentObj = processor.Undefined;
                }

                if (!ScriptProcessor.IsValidIdentifier(identifier))
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

                prototype.AddMember(processor, new PrototypeMember(identifier, contentObj));

                index = nextSeperatorIndex + 1;
            }

            return prototype.CreateInstance(processor, null, false);
        }
    }
}
