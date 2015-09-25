using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.Rendering
{
    class Material
    {
        public Material(Texture2D diffuseTexture)
        {
            DiffuseTexture = diffuseTexture;
            Color = Color.White;
            CastShadow = true;
            ReceiveShadow = true;
        }

        public Texture2D DiffuseTexture { get; set; }
        public Color Color { get; set; }
        public bool CastShadow { get; set; }
        public bool ReceiveShadow { get; set; }
    }
}
