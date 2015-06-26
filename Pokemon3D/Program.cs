#region File Information
/*
Program.cs

Pokémon 3D Open Source Version
Copyright © 2015 Nils Drescher. All rights reserved.
*/
#endregion

#region Using Statements
using System;
#endregion

namespace Pokemon3D
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameCore())
                game.Run();
        }
    }
}