using System;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 SnapToPixels(this Vector2 v)
        {
            return new Vector2((float)Math.Round(v.X), (float)Math.Round(v.Y));
        }

        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }
    }
}
