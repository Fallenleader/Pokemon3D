using System;
using System.Collections.Generic;
using System.Linq;
using Pokémon3D.Core;

/// <summary>
/// Visual Studio decides to not have any é in the namespace of the startup object.
/// So we just put "Pokemon3D" here and have Program.cs be in its own little namespace.
/// What a loner.
/// </summary>
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
            //Just running the game here currently:
            using (MainGame game = new MainGame())
                game.Run();
        }
    }
}