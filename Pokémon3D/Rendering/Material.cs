using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokémon3D.Rendering
{
    class Material
    {
        public Material(Texture2D diffuseTexture)
        {
            DiffuseTexture = diffuseTexture;
        }

        public Texture2D DiffuseTexture { get; set; }
    }
}
