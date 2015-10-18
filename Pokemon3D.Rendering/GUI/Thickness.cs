using System;

namespace Pokemon3D.Rendering.GUI
{
    public struct Thickness
    {
        public static readonly Thickness Empty = new Thickness();

        public int Top;
        public int Bottom;
        public int Left;
        public int Right;

        public int Vertical { get { return Top + Bottom; } }
        public int Horizontal { get { return Left + Right; } }

        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Thickness(int horizontal, int vertical)
        {
            Left = horizontal;
            Right = horizontal;
            Top = vertical;
            Bottom = vertical;
        }

        public Thickness(int distance)
        {
            Left = distance;
            Right = distance;
            Top = distance;
            Bottom = distance;
        }

        public static Thickness Parse(string text)
        {
            var token = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (token.Length == 1) return new Thickness(int.Parse(token[0]));
            if (token.Length == 2) return new Thickness(int.Parse(token[0]), int.Parse(token[1]));
            if (token.Length == 4) return new Thickness(int.Parse(token[0]), int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]));

            throw new ArgumentException("string is not parsable margin", "text");
        }
    }
}
