using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.Components
{
    interface IColorService
    {
        Color AccentMain { get; }

        Color AccentDark { get; }

        Color BackMain { get; }

        Color BackSub { get; }

        Color TextMain { get; }

        Color TextAccent { get; }
    }
}