using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using birdScript.Types;

namespace birdScript
{
    public class ScriptProcessor
    {
        #region Public interface

        

        #endregion

        internal SObject Undefined
        {
            get { return null; }
        }

        internal SObject Null
        {
            get { return null; }
        }

        internal SString CreateString(string value)
        {
            return null;
        }

        internal SNumber CreateNumber(double value)
        {
            return null;
        }

        internal SBool CreateBool(bool value)
        {
            return null;
        }
    }
}
