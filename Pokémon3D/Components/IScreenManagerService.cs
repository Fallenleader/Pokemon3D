using Microsoft.Xna.Framework;

namespace Pokémon3D.Components
{
    /// <summary>
    /// An interface for a screen implementation.
    /// </summary>
    interface IScreenManagerService
    {
        void Draw(GameTime gameTime);

        void Update(GameTime gameTime);

        Screens.Screen CurrentScreen();

        void SetScreen(Screens.Screen newScreen);
    }
}
