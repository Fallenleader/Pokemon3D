using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    public partial class ScriptProcessor
    {
        private const string REGEX_NUMRIGHTDOT = "^[0-9]+(E[-+][0-9]+)?$";
        private const string REGEX_NUMLEFTDOT = @"^[-]?\d+$";
        private const string REGEX_LAMBDA = @"^([a-zA-Z][a-zA-Z0-9]*([ ]*[,][ ]*[a-zA-Z][a-zA-Z0-9]*)*|\(\))[ ]*=>.+$";
    }
}
