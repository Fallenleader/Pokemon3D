#region File Information
/*
Constants.cs

Pokémon 3D Open Source Version
Copyright © 2015 Nils Drescher. All rights reserved.
*/
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Pokemon3D
{
    public partial class GameCore
    {
        /// <summary>
        /// The current version of the game.
        /// </summary>
        public const string GAMEVERSION = "1.0";

        /// <summary>
        /// The amount of versions released so far.
        /// </summary>
        public const string RELEASEVERSION = "88";

        /// <summary>
        /// The development stage the game is in.
        /// </summary>
        public const string GAMEDEVELOPMENTSTAGE = "OSV";

        /// <summary>
        /// The name of the game.
        /// </summary>
        public const string GAMENAME = "Pokémon 3D";

        /// <summary>
        /// The name of the developer of this game.
        /// </summary>
        public const string DEVELOPERNAME = "nilllzz Brand";

        //Controlling constants:

        /// <summary>
        /// If the game is running in Debug mode.
        /// </summary>
        public const bool DEBUG_ACTIVE = true;

    }
}