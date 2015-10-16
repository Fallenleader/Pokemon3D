using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript.Types
{
    class SVariable : SObject
    {
        private SObject _data;

        /// <summary>
        /// The identifier associated with this variable.
        /// </summary>
        public string Identifier { get; private set; }
        /// <summary>
        /// If the data of this variable cannot be set via script statements.
        /// </summary>
        public bool IsReadOnly { get; set; }

        public SVariable(string identifier, SObject data)
        {
            Identifier = identifier;
            _data = data;
        }

        public SVariable(string identifier, SObject data, bool isReadOnly)
        {
            Identifier = identifier;
            IsReadOnly = isReadOnly;
            _data = data;
        }

        /// <summary>
        /// The data this variable holds.
        /// </summary>
        public SObject Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (!IsReadOnly)
                {
                    _data = value;
                }
            }
        }

        /// <summary>
        /// Sets data ignoring the Read-Only property. Only use when necessary.
        /// </summary>
        public void ForceSetData(SObject data)
        {
            _data = data;
        }

        internal override string ToScriptObject()
        {
            return ObjectBuffer.GetObjectId(this).ToString();
        }

        #region Data proxy method overrides

        internal override SObject ExecuteMethod(ScriptProcessor processor, string methodName, SObject caller, SObject This, SObject[] parameters)
        {
            return Data.ExecuteMethod(processor, methodName, caller, This, parameters);
        }

        internal override SObject GetMember(ScriptProcessor processor, SObject accessor, bool isIndexer)
        {
            return Data.GetMember(processor, accessor, isIndexer);
        }

        internal override bool HasMember(ScriptProcessor processor, string memberName)
        {
            return Data.HasMember(processor, memberName);
        }

        internal override void SetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value)
        {
            Data.SetMember(processor, accessor, isIndexer, value);
        }

        internal override double SizeOf()
        {
            return Data.SizeOf();
        }

        internal override SBool ToBool(ScriptProcessor processor)
        {
            return Data.ToBool(processor);
        }

        internal override SNumber ToNumber(ScriptProcessor processor)
        {
            return Data.ToNumber(processor);
        }

        internal override SString ToString(ScriptProcessor processor)
        {
            return Data.ToString(processor);
        }

        internal override string TypeOf()
        {
            return Data.TypeOf();
        }

        internal override string ToScriptSource()
        {
            return Data.ToScriptSource();
        }

        #endregion
    }
}
