using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pokemon3D.UI
{
    /// <summary>
    /// Describes a 2D-triangle.
    /// </summary>
    struct Triangle : IShape, IEquatable<Triangle>
    {
        private Rectangle _bounds;
        private Point[] _points;

        /// <summary>
        /// Gets or sets the bounds of this <see cref="Triangle"/>.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        /// <summary>
        /// Gets or sets the location of this <see cref="Triangle"/>.
        /// </summary>
        public Point Location
        {
            get { return _bounds.Location; }
        }

        /// <summary>
        /// The points defining this <see cref="Triangle"/>.
        /// </summary>
        public Point[] Points
        {
            get { return _points; }
        }

        /// <summary>
        /// The first point of this <see cref="Triangle"/>.
        /// </summary>
        public Point A
        {
            get { return _points[0]; }
            set
            {
                _points[0] = value;
                calculateBounds();
            }
        }

        /// <summary>
        /// The second point of this <see cref="Triangle"/>.
        /// </summary>
        public Point B
        {
            get { return _points[1]; }
            set
            {
                _points[1] = value;
                calculateBounds();
            }
        }

        /// <summary>
        /// The third point of this <see cref="Triangle"/>.
        /// </summary>
        public Point C
        {
            get { return _points[2]; }
            set
            {
                _points[2] = value;
                calculateBounds();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Triangle"/> struct.
        /// </summary>
        /// <param name="a">The first point, count-clockwise rotation.</param>
        /// <param name="b">The second point, count-clockwise rotation.</param>
        /// <param name="c">The third point, count-clockwise rotation.</param>
        public Triangle(Point a, Point b, Point c)
             : this(new Point[] { a, b, c })
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="Triangle"/> struct.
        /// </summary>
        /// <param name="points">The three points defining the triangle.</param>
        public Triangle(Point[] points)
        {
            if (points.Length != 3)
                throw new ArgumentException("A triangle has to have three points.");

            _points = points;
            _bounds = Rectangle.Empty;

            calculateBounds();
        }

        private void calculateBounds()
        {
            //This recalculates the bounds of this triangle.
            //To do this, we need to find the top left and bottom right points:

            Point topLeft = new Point(_points.OrderBy(p => p.X).First().X, _points.OrderBy(p => p.Y).First().Y);
            Point bottomRight = new Point(_points.OrderBy(p => p.X).Last().X, _points.OrderBy(p => p.Y).Last().Y);

            _bounds = new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        private bool isPointInTriangle(int x, int y)
        {
            //The internal check if specific coordinates lie in this triangle.

            var a = _points[0];
            var b = _points[1];
            var c = _points[2];

            //We calculate the barycentric coordinates of our triangle with the point:
            var s = a.Y * c.X - a.X * c.Y + (c.Y - a.Y) * x + (a.X - c.X) * y;
            var t = a.X * b.Y - a.Y * b.X + (a.Y - b.Y) * x + (b.X - a.X) * y;

            //Then we check if those cooridinates satisfy this equation:
            //(x, y) = a + (b - a) * s + (c - a) * t

            if ((s < 0) != (t < 0))
                return false;

            var A = -b.Y * c.X + a.Y * (c.X - b.X) + a.X * (b.Y - c.Y) + b.X * c.Y;
            if (A < 0.0)
            {
                s = -s;
                t = -t;
                A = -A;
            }
            return s > 0 && t > 0 && (s + t) < A;
        }

        /// <summary>
        /// Returns the area of this <see cref="Triangle"/>.
        /// </summary>
        public double GetArea()
        {
            double area = 0;

            for (int i = 0; i < _points.Length; i++)
            {
                int i2 = i + 1;
                if (i2 > _points.Length - 1)
                    i2 = 0;

                double x0 = _points[i].X;
                double y0 = _points[i].Y;
                double x1 = _points[i2].X;
                double y1 = _points[i2].Y;

                area += (x0 - x1) * (y0 + y1) / 2;
            }

            return area;
        }

        /// <summary>
        /// Checks if a this <see cref="Triangle"/> contains a <see cref="Point"/>.
        /// </summary>
        public bool Contains(Point value)
        {
            return isPointInTriangle(value.X, value.Y);
        }

        /// <summary>
        /// Checks if a this <see cref="Triangle"/> contains a <see cref="Rectangle"/>.
        /// </summary>
        public bool Contains(Rectangle value)
        {
            if (!_bounds.Contains(value))
                return false;

            if (!isPointInTriangle(value.Left, value.Top))
                return false;

            if (!isPointInTriangle(value.Left, value.Bottom))
                return false;

            if (!isPointInTriangle(value.Right, value.Top))
                return false;

            if (!isPointInTriangle(value.Right, value.Bottom))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a this <see cref="Triangle"/> contains a <see cref="Vector2"/>.
        /// </summary>
        public bool Contains(Vector2 value)
        {
            return isPointInTriangle((int)value.X, (int)value.Y);
        }

        /// <summary>
        /// Checks if a this <see cref="Triangle"/> contains the specified coordinates.
        /// </summary>
        public bool Contains(int x, int y)
        {
            return isPointInTriangle(x, y);
        }

        /// <summary>
        /// Compares whether the current instance is equal to specified <see cref="Triangle"/>.
        /// </summary>
        public bool Equals(Triangle other)
        {
            return A == other.A &&
                   B == other.B &&
                   C == other.C;
        }

        /// <summary>
        /// Compares if this instance is equal to an object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Triangle ? Equals((Triangle)obj) : false;
        }

        /// <summary>
        /// Get the hash code of this <see cref="Triangle"/>.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 27;

            for (int i = 0; i < _points.Length; i++)
                hash = (13 * hash) + _points[i].GetHashCode();

            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Triangle"/>.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _points.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                sb.Append(i.ToString());
                sb.Append(": ");
                sb.Append(_points[i].ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compares whether two <see cref="Triangle"/> instances are equal.
        /// </summary>
        public static bool operator ==(Triangle left, Triangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares whether two <see cref="Triangle"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Triangle left, Triangle right)
        {
            return !left.Equals(right);
        }
    }
}