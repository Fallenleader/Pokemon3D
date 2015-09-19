using System;
using System.Collections.Generic;
using System.Linq;
using Pokémon3D.GameCore;

/// <summary>
/// Visual Studio decides to not have any é in the namespace of the startup object.
/// So we just put "Pokemon3D" here and have Program.cs be in its own little namespace.
/// </summary>
namespace Pokemon3D.GameCore
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
            //Just running the game here currently:
            using (GameController game = new GameController())
                game.Run();
        }
    }
}