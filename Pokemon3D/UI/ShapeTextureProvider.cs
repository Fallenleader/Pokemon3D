using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.UI
{
    /// <summary>
    /// A class to provide access to textures generated from shapes.
    /// </summary>
    static class ShapeTextureProvider
    {
        private static Dictionary<string, Texture2D> _tempTextures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Returns a texture generated from an <see cref="IShape"/>.
        /// </summary>
        /// <param name="shape">The shape to generate a texture from.</param>
        public static Texture2D GetTexture(GraphicsDevice device, IShape shape)
        {
            string comparer = string.Concat(new string[] { shape.GetType().Name, shape.ToString() });

            Texture2D texture;

            //Search for the texture in the dictionary, so it doesn't have to be created twice.
            if (_tempTextures.Keys.Contains(comparer))
            {
                texture = _tempTextures[comparer];
            }
            else
            {
                texture = createTexture(device, shape);
                _tempTextures.Add(comparer, texture);
            }

            return texture;
        }

        private static Texture2D createTexture(GraphicsDevice device, IShape shape)
        {
            //Create color array that contains the texture data:
            Color[] colorArr = new Color[shape.Bounds.Width * shape.Bounds.Height];

            for (int x = 0; x < shape.Bounds.Width; x++)
            {
                for (int y = 0; y < shape.Bounds.Height; y++)
                {
                    int index = y * shape.Bounds.Width + x;

                    //If the point is on the shape, set the color to white.
                    if (shape.Contains(x + shape.Bounds.X, y + shape.Bounds.Y))
                        colorArr[index] = Color.White;
                    else
                        colorArr[index] = Color.Transparent;
                }
            }

            //Create and fill the texture:
            Texture2D texture = new Texture2D(device, shape.Bounds.Width, shape.Bounds.Height);
            texture.SetData(colorArr);
            return texture;
        }
    }
}