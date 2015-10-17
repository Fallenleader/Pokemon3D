﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace birdScript.Types.Prototypes
{
    /// <summary>
    /// Represents an object from which other objects can be created.
    /// </summary>
    internal class Prototype : SProtoObject
    {
        internal string Name { get; }
        internal bool IsAbstract { get; private set; }
        internal PrototypeMember Constructor { get; private set; }
        internal Prototype Extends { get; private set; }

        private Dictionary<string, PrototypeMember> _prototypeMembers = new Dictionary<string, PrototypeMember>();
        private bool _initializedStatic;
        private SFunction _staticConstructor;
        private ScriptProcessor _staticConstructorProcessor;

        internal Prototype(string name)
        {
            Name = name;

            var methods = BuiltInMethodManager.GetMethods(GetType());
            foreach (var methodData in methods)
            {
                _prototypeMembers.Add(methodData.Item1, new PrototypeMember(
                        identifier: methodData.Item1,
                        data: new SFunction(methodData.Item3),
                        isStatic: methodData.Item2.IsStatic,
                        isReadOnly: false,
                        isIndexerGet: methodData.Item2.IsIndexerGet,
                        isIndexerSet: methodData.Item2.IsIndexerSet
                    ));
            }
        }

        internal override SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            InitializeStatic();

            bool isStaticCall = ReferenceEquals(caller, this);

            // Call any static function defined in this prototype:
            if (_prototypeMembers.ContainsKey(methodName))
            {
                if (_prototypeMembers[methodName].IsStatic && _prototypeMembers[methodName].IsFunction)
                {
                    var cFunction = (SFunction)_prototypeMembers[methodName].Data;
                    return cFunction.Call(processor, caller, This, parameters);
                }
                else
                {
                    return processor.ErrorHandler.ThrowError(ErrorType.TypeError, ErrorHandler.MESSAGE_TYPE_NOT_A_FUNCTION, new object[] { methodName });
                }
            }

            // Call the super class prototype, if one exists:
            if (Extends != null)
            {
                return Extends.ExecuteMethod(processor, methodName, caller, This, parameters);
            }

            return processor.ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MESSAGE_REFERENCE_NOT_DEFINED, new object[] { methodName });
        }

        internal override bool HasMember(ScriptProcessor processor, string memberName)
        {
            if (_prototypeMembers.ContainsKey(memberName))
                return true;
            else
                return base.HasMember(processor, memberName);
        }

        internal override SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            InitializeStatic();

            string memberName;
            if (accessor is SString)
                memberName = ((SString)accessor).Value;
            else
                memberName = accessor.ToString(processor).Value;

            if (_prototypeMembers.ContainsKey(memberName))
            {
                return _prototypeMembers[memberName].Data;
            }
            else
            {
                return processor.Undefined;
            }
        }

        internal override void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value)
        {
            InitializeStatic();

            string memberName;
            if (accessor is SString)
                memberName = ((SString)accessor).Value;
            else
                memberName = accessor.ToString(processor).Value;

            if (_prototypeMembers.ContainsKey(memberName))
            {
                if (_prototypeMembers[memberName].IsStatic && !_prototypeMembers[memberName].IsReadOnly)
                {
                    _prototypeMembers[memberName].Data = Unbox(value);
                }
            }
        }

        internal override string ToScriptSource()
        {
            return LITERAL_PROTOTYPE;
        }

        internal override string ToScriptObject()
        {
            return Name;
        }

        private void InitializeStatic()
        {
            // Runs the static constructor, if one exists.
            if (!_initializedStatic)
            {
                _initializedStatic = true;

                if (_staticConstructor != null)
                {
                    _staticConstructor.Call(_staticConstructorProcessor, this, this, new SObject[] { });
                }
            }
        }

        /// <summary>
        /// Creates the base object for this <see cref="Prototype"/>'s instantiation method.
        /// </summary>
        protected virtual SProtoObject CreateBaseObject()
        {
            return new SProtoObject();
        }

        /// <summary>
        /// Creates an instance derived from this prototype.
        /// </summary>
        internal SProtoObject CreateInstance(ScriptProcessor processor, SObject[] parameters, bool executeCtor)
        {
            SProtoObject obj = CreateBaseObject();

            obj.AddMember(MEMBER_NAME_PROTOTYPE, this);

            if (typeof(ObjectPrototype) != GetType())
            {
                // If no extends class is explicitly specified, "Object" is assumed.
                if (Extends == null)
                    Extends = processor.Context.GetPrototype("Object");

                var superInstance = Extends.CreateInstance(processor, null, false);
                obj.AddMember(MEMBER_NAME_SUPER, superInstance);
            }

            foreach (PrototypeMember member in GetInstanceMembers())
            {
                obj.AddMember(member.Identifier, member.Data);
            }

            var indexerGetFunction = GetIndexerGetFunction();
            if (indexerGetFunction != null)
                obj.IndexerGetFunction = indexerGetFunction.ToFunction();

            var indexerSetFunction = GetIndexerSetFunction();
            if (indexerSetFunction != null)
                obj.IndexerSetFunction = indexerSetFunction.ToFunction();

            if (executeCtor && Constructor != null)
            {
                Constructor.ToFunction().Call(processor, obj, obj, parameters);
            }

            // Lock all readonly members after the constructor call, so they can be set in the constructor:
            foreach (PrototypeMember member in GetReadOnlyInstanceMembers())
                obj.Members[member.Identifier].IsReadOnly = true;

            return obj;
        }

        private IEnumerable<PrototypeMember> GetInstanceMembers()
        {
            return _prototypeMembers.Where(x => !x.Value.IsStatic).Select(x => x.Value);
        }

        private IEnumerable<PrototypeMember> GetReadOnlyInstanceMembers()
        {
            return _prototypeMembers.Where(x => !x.Value.IsStatic && x.Value.IsReadOnly).Select(x => x.Value);
        }

        private PrototypeMember GetIndexerGetFunction()
        {
            foreach (var member in _prototypeMembers.Values)
            {
                if (member.IsIndexerGet && member.IsFunction)
                {
                    return member;
                }
            }
            return null;
        }

        private PrototypeMember GetIndexerSetFunction()
        {
            foreach (var member in _prototypeMembers.Values)
            {
                if (member.IsIndexerSet && member.IsFunction)
                {
                    return member;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns if the given member is static.
        /// </summary>
        internal bool IsStaticMember(string memberName)
        {
            if (_prototypeMembers.ContainsKey(memberName))
            {
                return _prototypeMembers[memberName].IsStatic;
            }
            return false;
        }

        /// <summary>
        /// Returns if the given member is readonly.
        /// </summary>
        internal bool IsReadOnlyMember(string memberName)
        {
            if (_prototypeMembers.ContainsKey(memberName))
            {
                return _prototypeMembers[memberName].IsReadOnly;
            }
            return false;
        }

        internal void AddMember(ScriptProcessor processor, PrototypeMember member)
        {
            if (_prototypeMembers.ContainsKey(member.Identifier))
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_DUPLICATE_DEFINITION, new object[] { member.Identifier, Name });

            _prototypeMembers.Add(member.Identifier, member);
        }

        private const string REGEX_CLASS_SIGNATURE = @"^class(([ ]+abstract)|([ ]+extends[ ]+[a-zA-Z]\w*)|([ ]+[a-zA-Z]\w*))+[ ]+{.*}$";

        private const string CLASS_SIGNATURE_EXTENDS = "extends";
        private const string CLASS_SIGNATURE_ABSTRACT = "abstract";
        private const string CLASS_METHOD_CTOR = "constructor";

        private const string FORMAT_VAR_ASSIGNMENT = "{0}={1};\n";

        internal new static SObject Parse(ScriptProcessor processor, string code)
        {
            code = code.Trim();

            if (Regex.IsMatch(code, REGEX_CLASS_SIGNATURE))
            {
                List<string> signature = code.Remove(code.IndexOf("{")).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string extends = "";
                string identifier = "";
                bool isAbstract = false;

                // Read extends:
                if (signature.Contains(CLASS_SIGNATURE_EXTENDS))
                {
                    int extendsIndex = signature.IndexOf(CLASS_SIGNATURE_EXTENDS);

                    if (extendsIndex + 1 == signature.Count) // when extends is the last element in the signature, throw error:
                        processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_EXTENDS_MISSING);

                    extends = signature[extendsIndex + 1]; // The extended class name is after the "extends" keyword.
                    signature.RemoveAt(extendsIndex); // Remove at the extends index twice, to remove the "extends" keyword and the identifier of the extended class.
                    signature.RemoveAt(extendsIndex);
                }

                // Read abstract:
                if (signature.Contains(CLASS_SIGNATURE_ABSTRACT))
                {
                    isAbstract = true;
                    signature.Remove(CLASS_SIGNATURE_ABSTRACT);
                }

                if (signature.Count != 2) // The signature must only have "class" and the identifier left.
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_INVALID_CLASS_SIGNATURE);

                // Read class name:
                identifier = signature[1];

                if (!ScriptProcessor.IsValidIdentifier(identifier))
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_IDENTIFIER_MISSING);

                // Create instance:
                Prototype prototype = new Prototype(identifier)
                {
                    IsAbstract = isAbstract
                };

                // Handle extends:
                if (extends.Length > 0)
                {
                    if (isAbstract)
                        processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_TYPE_ABSTRACT_NO_EXTENDS);

                    Prototype extendedPrototype = processor.Context.GetPrototype(extends);

                    if (extendedPrototype == null)
                        processor.ErrorHandler.ThrowError(ErrorType.ReferenceError, ErrorHandler.MESSAGE_REFERENCE_NO_PROTOTYPE, new object[] { extends });

                    prototype.Extends = extendedPrototype;
                }
                else
                {
                    // Set default prototype:
                    prototype.Extends = processor.Context.GetPrototype("Object");
                }

                string body = code.Remove(0, code.IndexOf("{") + 1);
                body = body.Remove(body.Length - 1, 1).Trim();

                string additionalCtorCode = "";
                string staticCtorCode = "";

                ScriptStatement[] statements = StatementProcessor.GetStatements(processor, body);

                foreach (ScriptStatement statement in statements)
                {
                    if (statement.StatementType == StatementType.Var)
                    {
                        var parsed = ParseVarStatement(processor, statement);

                        if (parsed.Item1.Identifier == CLASS_METHOD_CTOR)
                            processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);
                        
                        prototype.AddMember(processor, parsed.Item1);

                        if (parsed.Item2.Length > 0)
                        {
                            if (parsed.Item1.IsStatic)
                            {
                                staticCtorCode += string.Format(FORMAT_VAR_ASSIGNMENT, parsed.Item1.Identifier, parsed.Item2);
                            }
                            else
                            {
                                additionalCtorCode += string.Format(FORMAT_VAR_ASSIGNMENT, parsed.Item1.Identifier, parsed.Item2);
                            }
                        }
                    }
                    else if (statement.StatementType == StatementType.Function)
                    {
                        PrototypeMember parsed = ParseFunctionStatement(processor, statement);

                        if (parsed.Identifier == CLASS_METHOD_CTOR)
                        {
                            if (prototype.Constructor != null)
                                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_DUPLICATE_DEFINITION, new object[] { parsed.Identifier, identifier });

                            prototype.Constructor = parsed;
                        }
                        else
                        {
                            prototype.AddMember(processor, parsed);
                        }
                    }
                    else
                    {
                        processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_INVALID_STATEMENT);
                    }
                }

                // Add additional constructor code & static constructor code to prototype:

                if (staticCtorCode.Length > 0)
                {
                    prototype._staticConstructor = new SFunction(staticCtorCode, new string[] { });
                    prototype._staticConstructorProcessor = processor;
                }

                if (additionalCtorCode.Length > 0)
                {
                    if (prototype.Constructor == null)
                    {
                        // Create new ctor if no one has been defined:
                        prototype.Constructor = new PrototypeMember(CLASS_METHOD_CTOR, new SFunction("", new string[] { }), false, true, false, false);
                    }

                    prototype.Constructor.ToFunction().Body = additionalCtorCode + prototype.Constructor.ToFunction().Body;
                }

                return prototype;
            }
            else
            {
                return processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_INVALID_CLASS_SIGNATURE);
            }
        }

        private const string VAR_SIGNATURE_STATIC = "static";
        private const string VAR_SIGNATURE_READONLY = "readonly";

        private static Tuple<PrototypeMember, string> ParseVarStatement(ScriptProcessor processor, ScriptStatement statement)
        {
            string code = statement.Code;
            List<string> signature = code.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            string identifier;
            string assignment = "";
            bool isReadOnly = false;
            bool isStatic = false;

            // Read static:
            if (signature.Contains(VAR_SIGNATURE_STATIC))
            {
                isStatic = true;
                signature.Remove(VAR_SIGNATURE_STATIC);
            }

            // Read readonly:
            if (signature.Contains(VAR_SIGNATURE_READONLY))
            {
                isReadOnly = true;
                signature.Remove(VAR_SIGNATURE_READONLY);
            }

            if (signature[0] != "var" || signature.Count < 2)
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_INVALID_VAR_DECLARATION);

            identifier = signature[1];

            if (!ScriptProcessor.IsValidIdentifier(identifier))
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            if (signature.Count > 2)
            {
                if (signature[2].StartsWith("="))
                {
                    assignment = code.Remove(0, code.IndexOf("=") + 1).Trim();
                }
                else
                {
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_INVALID_VAR_DECLARATION);
                }
            }

            var member = new PrototypeMember(identifier, processor.Undefined, isStatic, isReadOnly, false, false);

            return new Tuple<PrototypeMember, string>(member, assignment);
        }

        private const string FUNCTION_SIGNATURE_STATIC = "static";
        private const string FUNCTION_SIGNATURE_INDEXER = "indexer";
        private const string FUNCTION_SIGNATURE_GET = "get";
        private const string FUNCTION_SIGNATURE_SET = "set";

        private static PrototypeMember ParseFunctionStatement(ScriptProcessor processor, ScriptStatement statement)
        {
            string code = statement.Code;
            List<string> signature = code.Remove(code.IndexOf("(")).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            string identifier;
            bool isStatic = false;
            bool isIndexerGet = false;
            bool isIndexerSet = false;

            // Read static:
            if (signature.Contains(FUNCTION_SIGNATURE_STATIC))
            {
                isStatic = true;
                signature.Remove(FUNCTION_SIGNATURE_STATIC);
            }

            // Read indexer:
            if (signature.Contains(FUNCTION_SIGNATURE_INDEXER))
            {
                int indexerIndex = signature.IndexOf(FUNCTION_SIGNATURE_INDEXER);

                if (indexerIndex + 1 == signature.Count)
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_FUNCTION_INDEXER_EXPECTED_TYPE);

                string indexerType = signature[indexerIndex + 1];
                if (indexerType == FUNCTION_SIGNATURE_GET)
                {
                    isIndexerGet = true;
                    signature.RemoveAt(indexerIndex + 1);
                }
                else if (indexerType == FUNCTION_SIGNATURE_SET)
                {
                    isIndexerSet = true;
                    signature.RemoveAt(indexerIndex + 1);
                }
                else
                {
                    processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_FUNCTION_INDEXER_INVALID_TYPE, new object[] { indexerType });
                }

                signature.Remove(FUNCTION_SIGNATURE_INDEXER);
            }

            if (signature.Count != 2)
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_CLASS_INVALID_FUNCTION_SIGNATURE);

            identifier = signature[1];

            if (!ScriptProcessor.IsValidIdentifier(identifier))
                processor.ErrorHandler.ThrowError(ErrorType.SyntaxError, ErrorHandler.MESSAGE_SYNTAX_MISSING_VAR_NAME);

            SFunction function = new SFunction(processor, signature[0] + code.Remove(0, code.IndexOf("(")));

            return new PrototypeMember(identifier, function, isStatic, false, isIndexerGet, isIndexerSet);
        }
    }
}