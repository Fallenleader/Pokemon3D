using System;
using Microsoft.Xna.Framework;

namespace Pokemon3D.UI
{
    /// <summary>
    /// Describes a 2D-plane.
    /// </summary>
    struct Plane : IShape, IEquatable<Plane>
    {
        private Rectangle _bounds;

        /// <summary>
        /// Gets or sets the bounds of this <see cref="Plane"/>.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the location of this <see cref="Plane"/>.
        /// </summary>
        public Point Location
        {
            get { return _bounds.Location; }
            set { _bounds.Location = value; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Plane"/> struct.
        /// </summary>
        public Plane(Rectangle bounds)
        {
            _bounds = bounds;
        }

        /// <summary>
        /// Returns the area of this <see cref="Plane"/>.
        /// </summary>
        public double GetArea()
        {
            return _bounds.Width * _bounds.Height;
        }

        /// <summary>
        /// Checks if a this <see cref="Plane"/> contains a <see cref="Point"/>.
        /// </summary>
        public bool Contains(Point value)
        {
            return _bounds.Contains(value);
        }

        /// <summary>
        /// Checks if a this <see cref="Plane"/> contains a <see cref="Rectangle"/>.
        /// </summary>
        public bool Contains(Rectangle value)
        {
            return _bounds.Contains(value);
        }

        /// <summary>
        /// Checks if a this <see cref="Plane"/> contains a <see cref="Vector2"/>.
        /// </summary>
        public bool Contains(Vector2 value)
        {
            return _bounds.Contains(value);
        }

        /// <summary>
        /// Checks if a this <see cref="Plane"/> contains the specified coordinates.
        /// </summary>
        public bool Contains(int x, int y)
        {
            return _bounds.Contains(x, y);
        }

        /// <summary>
        /// Compares whether the current instance is equal to specified <see cref="Plane"/>.
        /// </summary>
        public bool Equals(Plane other)
        {
            return _bounds.Equals(other._bounds);
        }

        /// <summary>
        /// Compares if this instance is equal to an object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Plane ? Equals((Plane)obj) : false;
        }

        /// <summary>
        /// Get the hash code of this <see cref="Plane"/>.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 27;
            hash = (hash * 13) + _bounds.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Plane"/>.
        /// </summary>
        public override string ToString()
        {
            return _bounds.ToString();
        }

        /// <summary>
        /// Compares whether two <see cref="Plane"/> instances are equal.
        /// </summary>
        public static bool operator ==(Plane left, Plane right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares whether two <see cref="Plane"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Plane left, Plane right)
        {
            return !left.Equals(right);
        }
    }
}