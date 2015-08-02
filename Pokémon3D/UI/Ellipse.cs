using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokémon3D.UI
{
    /// <summary>
    /// Describes a 2D-ellipse.
    /// </summary>
    struct Ellipse : IShape, IEquatable<Ellipse>
    {
        private Rectangle _bounds;

        /// <summary>
        /// Gets or sets the bounds of this <see cref="Ellipse"/>.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the location of this <see cref="Ellipse"/>.
        /// </summary>
        public Point Location
        {
            get { return _bounds.Location; }
            set { _bounds.Location = value; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Ellipse"/> struct.
        /// </summary>
        public Ellipse(int x, int y, int width, int height)
        {
            _bounds = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Ellipse"/> struct.
        /// </summary>
        public Ellipse(Rectangle bounds)
        {
            _bounds = bounds;
        }

        private bool isPointInEllipse(int x, int y)
        {
            //This is the internal check if x and y coordinates are inside the ellipse.

            double xRadius = _bounds.Width / 2d;
            double yRadius = _bounds.Height / 2d;
            Point normalized = new Point(x - _bounds.Center.X, y - _bounds.Center.Y);

            return ((normalized.X * normalized.X) / (xRadius * xRadius)) + ((normalized.Y * normalized.Y) / (yRadius * yRadius)) <= 1.0;
        }

        /// <summary>
        /// Returns the area of this <see cref="Ellipse"/>.
        /// </summary>
        public double GetArea()
        {
            return MathHelper.Pi * _bounds.Width * _bounds.Height;
        }

        /// <summary>
        /// Checks if a this <see cref="Ellipse"/> contains a <see cref="Point"/>.
        /// </summary>
        public bool Contains(Point value)
        {
            return isPointInEllipse(value.X, value.Y);
        }

        /// <summary>
        /// Checks if a this <see cref="Ellipse"/> contains a <see cref="Rectangle"/>.
        /// </summary>
        public bool Contains(Rectangle value)
        {
            //Check if the rectangle is inside the bounds.
            if (!_bounds.Contains(value))
                return false;

            //Check if the points of the rectangle are inside the ellipse.
            if (!isPointInEllipse(value.Left, value.Top))
                return false;

            if (!isPointInEllipse(value.Left, value.Bottom))
                return false;

            if (!isPointInEllipse(value.Right, value.Top))
                return false;

            if (!isPointInEllipse(value.Right, value.Bottom))
                return false;

            //If all corner points of the rectangle are inside the ellipse,
            //the rectangle also is.

            return true;
        }

        /// <summary>
        /// Checks if a this <see cref="Ellipse"/> contains a <see cref="Vector2"/>.
        /// </summary>
        public bool Contains(Vector2 value)
        {
            return isPointInEllipse((int)value.X, (int)value.Y);
        }

        /// <summary>
        /// Checks if a this <see cref="Ellipse"/> contains the specified coordinates.
        /// </summary>
        public bool Contains(int x, int y)
        {
            return isPointInEllipse(x, y);
        }

        /// <summary>
        /// Compares whether the current instance is equal to specified <see cref="Ellipse"/>.
        /// </summary>
        public bool Equals(Ellipse other)
        {
            return _bounds == other._bounds;
        }

        /// <summary>
        /// Compares if this instance is equal to an object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Ellipse ? Equals((Ellipse)obj) : false;
        }

        /// <summary>
        /// Get the hash code of this <see cref="Ellipse"/>.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 27;
            hash = (13 * hash) + _bounds.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Ellipse"/>.
        /// </summary>
        public override string ToString()
        {
            return _bounds.ToString();
        }

        /// <summary>
        /// Compares whether two <see cref="Ellipse"/> instances are equal.
        /// </summary>
        public static bool operator ==(Ellipse left, Ellipse right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares whether two <see cref="Ellipse"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Ellipse left, Ellipse right)
        {
            return !left.Equals(right);
        }
    }
}