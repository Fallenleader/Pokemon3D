using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameCore
{
    abstract class GameContextObject
    {
        public GameController Game => GameController.Instance;
    }
}
