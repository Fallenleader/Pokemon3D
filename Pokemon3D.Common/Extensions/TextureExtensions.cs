using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Common.Extensions
{
    public static class TextureExtensions
    {
        public static Vector2 GetCenter(this Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height) * 0.5f;
        }
    }
}
