using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokémon3D.UI
{
    /// <summary>
    /// A class to provide access to textures generated from gradients in the form of shapes.
    /// </summary>
    static class GradientTextureProvider
    {
        private static Dictionary<string, Texture2D> _tempTextures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Returns a gradient filled into a shape.
        /// </summary>
        public static Texture2D GetTexture(GraphicsDevice device, Gradient gradient, IShape shape)
        {
            string comparer = string.Concat(new string[] { shape.GetType().Name, shape.ToString(), gradient.ToString() });

            Texture2D texture;

            //Search for the texture in the dictionary, so it doesn't have to be created twice.
            if (_tempTextures.Keys.Contains(comparer))
            {
                texture = _tempTextures[comparer];
            }
            else
            {
                texture = createTexture(device, gradient, shape);
                _tempTextures.Add(comparer, texture);
            }

            return texture;
        }

        private static Texture2D createTexture(GraphicsDevice device, Gradient gradient, IShape shape)
        {
            //Set the size along which the gradient gets drawn:
            int uSize = shape.Bounds.Height;
            if (gradient.Horizontal)
                uSize = shape.Bounds.Width;

            double diffR, diffG, diffB, diffA;

            var fromColor = gradient.FromColor;
            var toColor = gradient.ToColor;

            //Calculate from and to color differences:
            diffR = (int)toColor.R - (int)fromColor.R;
            diffG = (int)toColor.G - (int)fromColor.G;
            diffB = (int)toColor.B - (int)fromColor.B;
            diffA = (int)toColor.A - (int)fromColor.A;

            //Set the steps of colors used. If the steps in the gradient config is smaller than 0, as many steps as possible are used.
            double stepCount = gradient.Steps;
            if (stepCount < 0)
                stepCount = uSize;

            float stepSize = (float)Math.Ceiling((float)(uSize / stepCount));

            Color[] colorArr = new Color[shape.Bounds.Width * shape.Bounds.Height];

            int cR, cG, cB, cA;

            int length = (int)Math.Ceiling(stepSize);
            for (int cStep = 1; cStep <= stepCount; cStep++)
            {
                //Get the color for the current frame.
                cR = (int)(((diffR / stepCount) * cStep) + (int)fromColor.R);
                cG = (int)(((diffG / stepCount) * cStep) + (int)fromColor.G);
                cB = (int)(((diffB / stepCount) * cStep) + (int)fromColor.B);
                cA = (int)(((diffA / stepCount) * cStep) + (int)fromColor.A);

                //If the color overflows, it was calculated from the wrong side, ie black to white instead of white to black.
                if (cR < 0)
                    cR += 255;
                if (cG < 0)
                    cG += 255;
                if (cB < 0)
                    cB += 255;
                if (cA < 0)
                    cA += 255;

                Color c = new Color(cR, cG, cB, cA);
                int start = (int)((cStep - 1) * stepSize);

                if (gradient.Horizontal)
                {

                    for (int x = start; x < start + length; x++)
                    {
                        for (int y = 0; y < shape.Bounds.Height; y++)
                        {
                            int i = x + y * shape.Bounds.Width;
                            if (shape.Contains(x + shape.Location.X, y + shape.Location.Y))
                            {
                                colorArr[i] = c;
                            }
                            else
                            {
                                colorArr[i] = Color.Transparent;
                            }
                        }
                    }
                }
                else
                {
                    for (int y = start; y < start + length; y++)
                    {
                        for (int x = 0; x < shape.Bounds.Width; x++)
                        {
                            int i = x + y * shape.Bounds.Width;
                            if (shape.Contains(x + shape.Location.X, y + shape.Location.Y))
                            {
                                colorArr[i] = c;
                            }
                            else
                            {
                                colorArr[i] = Color.Transparent;
                            }
                        }
                    }
                }
            }

            //Create and fill texture:
            Texture2D texture = new Texture2D(device, shape.Bounds.Width, shape.Bounds.Height);
            texture.SetData(colorArr);
            return texture;
        }
    }
}