using System;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Extensions
{
    public static class StringParseExtension
    {
        public static Rectangle ParseRectangle(this string value)
        {
            var token = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (token.Length < 4) return new Rectangle();

            return new Rectangle(int.Parse(token[0]), int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]));
        }
    }
}
