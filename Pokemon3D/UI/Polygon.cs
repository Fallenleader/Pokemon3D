using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.UI
{
    /// <summary>
    /// Describes a 2D-polygon.
    /// </summary>
    struct Polygon : IShape, IEquatable<Polygon>
    {
        private Rectangle _bounds;
        private List<Point> _points;

        /// <summary>
        /// Gets the bounds of this <see cref="Polygon"/>.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        /// <summary>
        /// Gets or sets the location of this <see cref="Polygon"/>.
        /// </summary>
        public Point Location
        {
            get { return _bounds.Location; }
            set
            {
                Point offset = value - _bounds.Location;

                for (int i = 0; i < _points.Count; i++)
                    _points[i] += offset;

                _bounds.Location = value;
            }
        }

        /// <summary>
        /// The points defining this <see cref="Polygon"/>.
        /// </summary>
        public Point[] Points
        {
            get { return _points.ToArray(); }
        }

        /// <summary>
        /// The length of this <see cref="Polygon"/>.
        /// </summary>
        public int Length
        {
            get { return _points.Count; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Polygon"/> struct.
        /// </summary>
        /// <param name="points">The points defining this <see cref="Polygon"/>.</param>
        public Polygon(Point[] points)
        {
            if (points.Length < 3)
                throw new ArgumentException("A Polygon has to consist of at least three vertices.");

            _points = points.ToList();
            _bounds = Rectangle.Empty;

            calculateBounds();
        }

        private void calculateBounds()
        {
            //This recalculates the bounds of this polygon.
            //To do this, we need to find the top left and bottom right points:

            Point topLeft = new Point(_points.OrderBy(p => p.X).First().X, _points.OrderBy(p => p.Y).First().Y);
            Point bottomRight = new Point(_points.OrderBy(p => p.X).Last().X, _points.OrderBy(p => p.Y).Last().Y);

            _bounds = new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        private bool isPointInPolygon(int x, int y)
        {
            //This checks if a given point is inside a polygon.

            bool isInside = false;

            for (int i = 0, j = _points.Count - 1; i < _points.Count; j = i++)
            {
                if (((_points[i].Y > y) != (_points[j].Y > y)) &&
                (x < (_points[j].X - _points[i].X) * (y - _points[i].Y) / (_points[j].Y - _points[i].Y) + _points[i].X))
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }

        /// <summary>
        /// Removes a point from the point enumeration.
        /// </summary>
        /// <param name="index">The index where to remove the point from.</param>
        public void Remove(int index)
        {
            if (_points.Count <= index || index < 0)
                throw new IndexOutOfRangeException();
            else if (_points.Count == 3)
                throw new ArgumentException("A polygon has to consist of at least three vertices.");
            else
            {
                _points.RemoveAt(index);
                calculateBounds();
            }
        }

        /// <summary>
        /// Adds a new point to the end of the point enumeration.
        /// </summary>
        public void Add(Point point)
        {
            _points.Add(point);
            calculateBounds();
        }

        /// <summary>
        /// Inserts a new point into the points enumeration.
        /// </summary>
        public void Insert(int index, Point point)
        {
            _points.Insert(index, point);
            calculateBounds();
        }

        /// <summary>
        /// Returns the area of this <see cref="Polygon"/>.
        /// </summary>
        public double GetArea()
        {
            double area = 0;

            for (int i = 0; i < _points.Count; i++)
            {
                int i2 = i + 1;
                if (i2 > _points.Count - 1)
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
        /// Checks if a this <see cref="Polygon"/> contains a <see cref="Point"/>.
        /// </summary>
        public bool Contains(Point value)
        {
            return isPointInPolygon(value.X, value.Y);
        }

        /// <summary>
        /// Checks if a this <see cref="Polygon"/> contains a <see cref="Rectangle"/>.
        /// </summary>
        public bool Contains(Rectangle value)
        {
            //Check if the rectangle is inside the bounds.
            if (!_bounds.Contains(value))
                return false;

            //Check if the points of the rectangle are inside the polygon.
            if (!isPointInPolygon(value.Left, value.Top))
                return false;

            if (!isPointInPolygon(value.Left, value.Bottom))
                return false;

            if (!isPointInPolygon(value.Right, value.Top))
                return false;

            if (!isPointInPolygon(value.Right, value.Bottom))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a this <see cref="Polygon"/> contains a <see cref="Vector2"/>.
        /// </summary>
        public bool Contains(Vector2 value)
        {
            return isPointInPolygon((int)value.X, (int)value.Y);
        }

        /// <summary>
        /// Checks if a this <see cref="Polygon"/> contains the specified coordinates.
        /// </summary>
        public bool Contains(int x, int y)
        {
            return isPointInPolygon(x, y);
        }

        /// <summary>
        /// Compares whether the current instance is equal to specified <see cref="Polygon"/>.
        /// </summary>
        public bool Equals(Polygon other)
        {
            if (other.Length != Length)
                return false;

            for (int i = 0; i < _points.Count; i++)
                if (_points[i] != other._points[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Compares if this instance is equal to an object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Polygon ? Equals((Polygon)obj) : false;
        }

        /// <summary>
        /// Get the hash code of this <see cref="Polygon"/>.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 27;

            for (int i = 0; i < _points.Count; i++)
                hash = (13 * hash) + _points[i].GetHashCode();

            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Polygon"/>.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _points.Count; i++)
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
        /// Compares whether two <see cref="Polygon"/> instances are equal.
        /// </summary>
        public static bool operator ==(Polygon left, Polygon right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares whether two <see cref="Polygon"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Polygon left, Polygon right)
        {
            return !left.Equals(right);
        }
    }
}