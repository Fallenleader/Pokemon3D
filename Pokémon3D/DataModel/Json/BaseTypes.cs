using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json
{
    /// <summary>
    /// The data model for an RGB color.
    /// </summary>
    [DataContract]
    class ColorModel : JsonDataModel
    {
        /// <summary>
        /// The red part of the color.
        /// </summary>
        [DataMember(Order = 0)]
        public byte Red;

        /// <summary>
        /// The green part of the color.
        /// </summary>
        [DataMember(Order = 1)]
        public byte Green;

        /// <summary>
        /// The blue part of the color.
        /// </summary>
        [DataMember(Order = 2)]
        public byte Blue;

        /// <summary>
        /// Returns the <see cref="Color"/> corresponding to this model.
        /// </summary>
        public Color GetColor()
        {
            return new Color(Red, Green, Blue);
        }
    }

    /// <summary>
    /// The data model for a rectangle definition.
    /// </summary>
    [DataContract]
    class RectangleModel : JsonDataModel
    {
        /// <summary>
        /// The x position of this rectangle model.
        /// </summary>        
        [DataMember(Order = 0)]
        public int X;

        /// <summary>
        /// The y position of this rectangle model.
        /// </summary>        
        [DataMember(Order = 1)]
        public int Y;

        /// <summary>
        /// The width of this rectangle model.
        /// </summary>        
        [DataMember(Order = 2)]
        public int Width;

        /// <summary>
        /// The height of this rectangle model.
        /// </summary>        
        [DataMember(Order = 3)]
        public int Height;

        /// <summary>
        /// Returns the corresponding <see cref="Rectangle"/> to this model.
        /// </summary>
        public Rectangle GetRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }

    /// <summary>
    /// The data model for a <see cref="Vector3"/> definition.
    /// </summary>
    [DataContract]
    class Vector3Model : JsonDataModel
    {
        /// <summary>
        /// The X coordinate of this vector.
        /// </summary>
        [DataMember(Order = 0)]
        public float X;

        /// <summary>
        /// The Y coordinate of this vector.
        /// </summary>
        [DataMember(Order = 1)]
        public float Y;

        /// <summary>
        /// The Z coordinate of this vector.
        /// </summary>
        [DataMember(Order = 2)]
        public float Z;

        /// <summary>
        /// Returns the corresponding <see cref="Vector3"/> to this model.
        /// </summary>
        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }

    /// <summary>
    /// The data model for a <see cref="Vector2"/> definition.
    /// </summary>
    [DataContract]
    class Vector2Model : JsonDataModel
    {
        /// <summary>
        /// The X coordinate of this vector.
        /// </summary>
        [DataMember(Order = 0)]
        public float X;

        /// <summary>
        /// The Y coordinate of this vector.
        /// </summary>
        [DataMember(Order = 1)]
        public float Y;

        /// <summary>
        /// Returns the corresponding <see cref="Vector2"/> to this model.
        /// </summary>
        public Vector2 GetVector2()
        {
            return new Vector2(X, Y);
        }
    }

    /// <summary>
    /// The data model for a range.
    /// </summary>
    [DataContract]
    class RangeModel : JsonDataModel
    {
        /// <summary>
        /// The lower bound of the range.
        /// </summary>
        [DataMember(Order = 0)]
        public double Min;

        /// <summary>
        /// The upper bound of the range.
        /// </summary>
        [DataMember(Order = 1)]
        public double Max;
    }

    /// <summary>
    /// Describes a texture source.
    /// </summary>
    [DataContract]
    class TextureSourceModel : JsonDataModel
    {
        /// <summary>
        /// The source file for this texture.
        /// </summary>
        [DataMember(Order = 0)]
        public string Source;

        /// <summary>
        /// The rectangle cut out of the source texture.
        /// </summary>
        [DataMember(Order = 1)]
        public RectangleModel Rectangle;
    }
}