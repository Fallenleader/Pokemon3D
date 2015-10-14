using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;
using birdScript.Adapters;

namespace birdScript
{
    /// <summary>
    /// Adds various context elements to a <see cref="ScriptProcessor"/> class instance.
    /// </summary>
    public class ScriptContext
    {
        /// <summary>
        /// The parent context to this context. This context takes priority over its parent, if identifiers overlap.
        /// </summary>
        internal ScriptContext Parent { get; private set; }
        /// <summary>
        /// The object that gets returned when the script references "this".
        /// </summary>
        internal SObject This { get; set; }
        
        private Dictionary<CallbackType, Delegate> _apiCallbacks = new Dictionary<CallbackType, Delegate>();

        private List<SAPIUsing> _APIUsings = new List<SAPIUsing>();
        private Dictionary<string, SVariable> _variables = new Dictionary<string, SVariable>();
        private Dictionary<string, Prototype> _prototypes = new Dictionary<string, Prototype>();
        private ScriptProcessor _processor;

        #region Public interface

        // Public ctor, do not initialize anything, this is getting done when the context is passed into a processor instance.
        /// <summary>
        /// Creates a new instance of the <see cref="ScriptContext"/> class.
        /// </summary>
        public ScriptContext() { }

        /// <summary>
        /// Sets the callback for checking if an API class has a member.
        /// </summary>
        public void SetCallbackHasMember(DHasMember callback)
        {
            AddCallback(CallbackType.HasMember, callback);
        }

        /// <summary>
        /// Sets the callback for getting a member of an API class.
        /// </summary>
        public void SetCallbackGetMember(DGetMember callback)
        {
            AddCallback(CallbackType.GetMember, callback);
        }

        #endregion

        internal ScriptContext(ScriptProcessor processor, ScriptContext parent)
        {
            _processor = processor;
            Parent = parent;
        }

        internal void Initialize()
        {
            if (Parent != null)
            {
                AddVariable(SObject.LITERAL_UNDEFINED, SUndefined.Factory(), true);
                AddVariable(SObject.LITERAL_NULL, SNull.Factory(), true);
            }
        }

        private void AddCallback(CallbackType callbackType, Delegate callback)
        {
            // Adds or replaces a delegate in the delegate list.

            if (_apiCallbacks.ContainsKey(callbackType))
                _apiCallbacks.Add(callbackType, callback);
            else
                _apiCallbacks[callbackType] = callback;
        }

        internal bool HasCallback(CallbackType callbackType)
        {
            if (_apiCallbacks.ContainsKey(callbackType))
            {
                return true;
            }
            else
            {
                if (Parent != null)
                {
                    return Parent.HasCallback(callbackType);
                }
                else
                {
                    return false;
                }
            }
        }

        internal Delegate GetCallback(CallbackType callbackType)
        {
            if (_apiCallbacks.ContainsKey(callbackType))
            {
                return _apiCallbacks[callbackType];
            }
            else
            {
                if (Parent != null)
                {
                    return Parent.GetCallback(callbackType);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns if this context has a variable with a given identifier defined.
        /// </summary>
        internal bool IsVariable(string identifier)
        {
            if (_variables.Keys.Contains(identifier))
            {
                return true;
            }
            else
            {
                if (Parent != null)
                {
                    return Parent.IsVariable(identifier);
                }
            }
            return false;
        }

        internal SVariable GetVariable(string identifier)
        {
            if (_variables.Keys.Contains(identifier))
            {
                return _variables[identifier];
            }
            else
            {
                if (Parent != null)
                {
                    return Parent.GetVariable(identifier);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Adds a variable to the context.
        /// </summary>
        internal void AddVariable(string identifier, SObject value)
        {
            _variables.Add(identifier, new SVariable(identifier, value));
        }

        /// <summary>
        /// Adds a variable to the context and sets the readonly property.
        /// </summary>
        internal void AddVariable(string identifier, SObject value, bool isReadOnly)
        {
            _variables.Add(identifier, new SVariable(identifier, value, isReadOnly));
        }
    }
}
