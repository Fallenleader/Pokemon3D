using System;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Extensions
{
    public static class RectangleExtension
    {
        public static Rectangle Translate(this Rectangle rectangle, int x, int y)
        {
            return new Rectangle(rectangle.X +x, rectangle.Y + y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle Scale(this Rectangle rectangle, float scale)
        {
            return new Rectangle(rectangle.X, rectangle.Y, (int)Math.Round(rectangle.Width * scale), (int)Math.Round(rectangle.Height * scale));
        }
    }
}
