using System;

namespace Pokémon3D.GameCore
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //Just running the game here currently:
            using (GameController game = new GameController())
            {
                game.Run();
            }
        }
    }
}