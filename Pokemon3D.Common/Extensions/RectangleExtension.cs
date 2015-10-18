using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Extensions
{
    public static class RectangleExtension
    {
        public static Rectangle Translate(this Rectangle rectangle, int x, int y)
        {
            return new Rectangle(rectangle.X +x, rectangle.Y + y, rectangle.Width, rectangle.Height);
        }
    }
}
