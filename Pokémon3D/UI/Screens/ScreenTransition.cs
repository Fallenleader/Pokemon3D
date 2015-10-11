using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.UI.Screens
{
    interface ScreenTransition
    {
        void StartTransition(Texture2D source, Texture2D target);

        bool IsFinished { get; }

        void Update(float elapsedTimeSeconds);

        void Draw();
    }
}
