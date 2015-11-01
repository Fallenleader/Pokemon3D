using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// A component of a <see cref="GameMode"/>.
    /// </summary>
    interface IGameModeComponent
    {
        /// <summary>
        /// Gets called when the <see cref="GameMode"/> frees all resources.
        /// </summary>
        void FreeResources();

        /// <summary>
        /// Gets called when this component gets added to a <see cref="GameMode"/>.
        /// </summary>
        void Activated(GameMode gameMode);
    }
}
