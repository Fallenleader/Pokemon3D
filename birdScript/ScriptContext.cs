using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;
using birdScript.Types.Prototypes;

namespace birdScript
{
    public class ScriptContext
    {
        internal ScriptContext Parent { get; private set; }
        internal SObject This { get; set; }

        private List<string> _APIUsings = new List<string>();
        private Dictionary<string, SVariable> _variables = new Dictionary<string, SVariable>();
        private Dictionary<string, Prototype> _prototypes = new Dictionary<string, Prototype>();
        private ScriptProcessor _processor;
        
        public ScriptContext(ScriptProcessor processor)
        {
            _processor = processor;
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

        internal void AddVariable(string identifier, SObject value)
        {
            _variables.Add(identifier, new SVariable(identifier, value));
        }
    }
}
