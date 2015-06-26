#region File Information
/*
JsonDataModel.cs

Pokémon 3D Open Source Version
Copyright © 2015 Nils Drescher. All rights reserved.
*/
#endregion

#region Using Statements
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework;
#endregion

namespace Pokemon3D.DataModel.Json
{
    /// <summary>
    /// The base data model class.
    /// </summary>
    [DataContract]
    class JsonDataModel
    {
        protected JsonDataModel() {  /* Empty constructor */}

        /// <summary>
        /// Creates a data model of a specific type.
        /// </summary>
        /// <param name="input">The Json input string.</param>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        public static T FromString<T>(string input)
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memStream = new MemoryStream();

            //Create StreamWriter to the memory stream, which writes the input string to the stream.
            var sw = new StreamWriter(memStream);
            sw.Write(input);
            sw.Flush();

            //Reset the stream's position to the beginning:
            memStream.Position = 0;

            try
            {
                //Create and return the object:
                T returnObj = (T)serializer.ReadObject(memStream);
                return returnObj;
            }
            catch (Exception)
            {
                //Exception occurs while loading the object due to malformed Json.
                //Throw exception and move up to handler class.
                throw new JsonDataLoadException(input, typeof(T));
            }
        }

        /// <summary>
        /// Returns the Json representation of this object.
        /// </summary>
        public override string ToString()
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(this.GetType());
            var memStream = new MemoryStream();

            //Write the data to the stream.
            serializer.WriteObject(memStream, this);

            //Reset the stream's position to the beginning:
            memStream.Position = 0;

            //Create stream reader, read string and return it.
            var sr = new StreamReader(memStream);
            string returnJson = sr.ReadToEnd();

            return returnJson;
        }
    }

    /// <summary>
    /// An exception thrown when an error occurs while loading Json data.
    /// </summary>
    class JsonDataLoadException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="JsonDataLoadException"/> class.
        /// </summary>
        public JsonDataLoadException(string jsonData, Type targetType) : base("An exception occured trying to read Json data into an internal format. Please check that the input data is correct.")
        {
            Data.Add("Target type", targetType.Name);
            Data.Add("Json data", jsonData);
        }
    }

    #region Base data model definitions

    /// <summary>
    /// A data model for an RGB <see cref="Color"/> (no alpha).
    /// </summary>
    [DataContract]
    class RGBColorModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public byte Red;

        [DataMember(Order = 1)]
        public byte Green;

        [DataMember(Order = 2)]
        public byte Blue;

        /// <summary>
        /// Returns the <see cref="Color"/> of this model.
        /// </summary>
        public Color GetColor()
        {
            return new Color(Red, Green, Blue);
        }
    }

    /// <summary>
    /// A data model for a <see cref="Rectangle"/>.
    /// </summary>
    [DataContract]
    class RectangleModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public int X;

        [DataMember(Order = 1)]
        public int Y;

        [DataMember(Order = 2)]
        public int Width;

        [DataMember(Order = 3)]
        public int Height;

        /// <summary>
        /// Returns the <see cref="Rectangle"/> of this model.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }

    /// <summary>
    /// A data model for a <see cref="Vector3"/>.
    /// </summary>
    [DataContract]
    class Vector3Model : JsonDataModel
    {
        [DataMember(Order = 0)]
        public float X;

        [DataMember(Order = 1)]
        public float Y;

        [DataMember(Order = 2)]
        public float Z;

        /// <summary>
        /// Returns the <see cref="Vector3"/> of this model.
        /// </summary>
        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }

    /// <summary>
    /// A data model for a <see cref="Vector2"/>.
    /// </summary>
    [DataContract]
    class Vector2Model : JsonDataModel
    {
        [DataMember(Order = 0)]
        public float X;

        [DataMember(Order = 1)]
        public float Y;

        /// <summary>
        /// Returns the <see cref="Vector2"/> of this model.
        /// </summary>
        public Vector2 GetVector2()
        {
            return new Vector2(X, Y);
        }
    }

    /// <summary>
    /// A data model for a range.
    /// </summary>
    [DataContract]
    class RangeModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public decimal Min;

        [DataMember(Order = 1)]
        public decimal Max;
    }

    /// <summary>
    /// A data model for a texture rectangle in a texture atlas.
    /// </summary>
    [DataContract]
    class TextureSourceModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Source;

        [DataMember(Order = 1)]
        public RectangleModel Rectangle;
    }

    #endregion
}