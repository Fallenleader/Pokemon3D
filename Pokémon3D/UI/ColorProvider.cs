using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokémon3D.UI
{
    class BasicColorProvider : GameComponent, Components.IColorService
    {
        public BasicColorProvider(Core.MainGame game) : base(game)
        {
            game.Services.AddService(typeof(Components.IColorService), this);
        }

        private static Color accentMain = new Color(255, 136, 20);
        private static Color accentDark = new Color(218, 68, 22);
        private static Color backMain = new Color(240, 240, 240);
        private static Color backSub = new Color(227, 227, 227);
        private static Color textMain = new Color(25, 25, 25);
        private static Color textAccent = new Color(240, 240, 240);

        public Color AccentMain
        {
            get { return accentMain; }
        }

        public Color AccentDark
        {
            get { return accentDark; }
        }

        public Color BackMain
        {
            get { return backMain; }
        }

        public Color BackSub
        {
            get { return backSub; }
        }

        public Color TextMain
        {
            get { return textMain; }
        }

        public Color TextAccent
        {
            get { return textAccent; }
        }
    }
}