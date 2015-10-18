using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Extensions
{
    public static class PointExtension
    {
        public static Point Left(this Point p)
        {
            return new Point(p.X - 1, p.Y);
        }

        public static Point Right(this Point p)
        {
            return new Point(p.X + 1, p.Y);
        }

        public static Point Up(this Point p)
        {
            return new Point(p.X, p.Y - 1);
        }

        public static Point Down(this Point p)
        {
            return new Point(p.X, p.Y + 1);
        }

        public static Point Offset(this Point p, int dx, int dy)
        {
            return new Point(p.X + dx, p.Y + dy);
        }

        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}
