using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.UI
{
    /// <summary>
    /// Represents a 2D-gradient.
    /// </summary>
    struct Gradient : IEquatable<Gradient>
    {
        private Color _fromColor, _toColor;
        private bool _horizontal;
        private int _steps;

        /// <summary>
        /// The first color of the <see cref="Gradient"/>.
        /// </summary>
        public Color FromColor
        {
            get { return _fromColor; }
            set { _fromColor = value; }
        }

        /// <summary>
        /// The second color of the <see cref="Gradient"/>.
        /// </summary>
        public Color ToColor
        {
            get { return _toColor; }
            set { _toColor = value; }
        }

        /// <summary>
        /// Indicates wether this <see cref="Gradient"/> is oriented horizontally.
        /// </summary>
        public bool Horizontal
        {
            get { return _horizontal; }
            set { _horizontal = value; }
        }

        /// <summary>
        /// The steps this <see cref="Gradient"/> takes. If set to -1, the <see cref="Gradient"/> takes as many steps as possible.
        /// </summary>
        public int Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Gradient"/> struct.
        /// </summary>
        public Gradient(Color fromColor, Color toColor, bool horizontal, int steps)
        {
            if (steps == 0 || steps < -1)
                throw new ArgumentException("Steps have to be larger than 0, or -1.");

            _fromColor = fromColor;
            _toColor = toColor;
            _horizontal = horizontal;
            _steps = steps;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Gradient"/> struct.
        /// </summary>
        public Gradient(Color fromColor, Color toColor, bool horizontal)
            : this(fromColor, toColor, horizontal, -1)
        { }

        /// <summary>
        /// Compares whether the current instance is equal to specified <see cref="Gradient"/>.
        /// </summary>
        public bool Equals(Gradient other)
        {
            return _fromColor == other._fromColor &&
                _toColor == other._toColor &&
                _horizontal == other._horizontal &&
                _steps == other._steps;
        }

        /// <summary>
        /// Compares if this instance is equal to an object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Gradient ? Equals((Gradient)obj) : false;
        }

        /// <summary>
        /// Get the hash code of this <see cref="Gradient"/>.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 27;

            hash = (13 * hash) + _fromColor.GetHashCode();
            hash = (13 * hash) + _toColor.GetHashCode();
            hash = (13 * hash) + _horizontal.GetHashCode();
            hash = (13 * hash) + _steps.GetHashCode();

            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Gradient"/>.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("FromColor: ");
            sb.Append(_fromColor.ToString());
            sb.Append("; ToColor: ");
            sb.Append(_toColor.ToString());
            sb.Append("; Horizontal: ");
            sb.Append(_horizontal.ToString());
            sb.Append("; Steps: ");
            sb.Append(_steps.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Compares whether two <see cref="Gradient"/> instances are equal.
        /// </summary>
        public static bool operator ==(Gradient left, Gradient right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares whether two <see cref="Gradient"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Gradient left, Gradient right)
        {
            return !left.Equals(right);
        }
    }
}