using Microsoft.Xna.Framework;

namespace Pokémon3D.UI
{
    /// <summary>An interface for standard shapes.</summary>
    interface IShape
    {
        /// <summary>The outer bounds of this shape.</summary>
        Rectangle Bounds { get; }

        /// <summary>The location of this shape.</summary>
        Point Location { get; }

        /// <summary>Calculates the area of this shape.</summary>
        double GetArea();

        /// <summary>Returns if this shape contains the x and y coodinates.</summary>
        bool Contains(int x, int y);

        /// <summary>Returns if this shape contains a <see cref="Vector2"/>.</summary>
        bool Contains(Vector2 value);

        /// <summary>Returns if this shape contains a <see cref="Rectangle"/>.</summary>
        bool Contains(Rectangle value);

        /// <summary>Returns if this shape contains a <see cref="Point"/>.</summary>
        bool Contains(Point value);
    }
}
