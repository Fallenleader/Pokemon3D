using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Pokemon3D.DataModel.Json
{
    /// <summary>
    /// The base data model class.
    /// </summary>
    [DataContract]
    abstract class JsonDataModel
    {
        /// <summary>
        /// Creates a data model of a specific type, loaded from a file.
        /// </summary>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        public static T FromFile<T>(string fileName)
        {
            if (File.Exists(fileName))
                return FromString<T>(File.ReadAllText(fileName));
            else
                throw new FileNotFoundException("The Json file does not exist.", fileName);
        }

        /// <summary>
        /// Creates a data model of a specific type.
        /// </summary>
        /// <typeparam name="T">The return type of the data model.</typeparam>
        /// <param name="input">The Json input string.</param>
        public static T FromString<T>(string input)
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings() { SerializeReadOnlyTypes = true });
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
            catch (Exception ex)
            {
                //Exception occurs while loading the object due to malformed Json.
                //Throw exception and move up to handler class.
                throw new JsonDataLoadException(ex, input, typeof(T));
            }
        }

        /// <summary>
        /// Returns the Json representation of this object.
        /// </summary>
        public override string ToString()
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(GetType(), new DataContractJsonSerializerSettings() { SerializeReadOnlyTypes = true });
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

        /// <summary>
        /// Converts a <see cref="string"/> to a member of the given enum type.
        /// </summary>
        protected static TEnum ConvertStringToEnum<TEnum>(string enumString)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), enumString, true);
        }

        /// <summary>
        /// Converts a <see cref="string"/> array to an array of the given enum type.
        /// </summary>
        protected static ICollection<TEnum> ConvertStringCollectionToEnumCollection<TEnum>(ICollection<string> enumMemberArray)
        {
            TEnum[] enumArr = new TEnum[enumMemberArray.Count];

            for (int i = 0; i < enumMemberArray.Count; i++)
                enumArr[i] = ConvertStringToEnum<TEnum>(enumMemberArray.ElementAt(i));

            return enumArr;
        }

        /// <summary>
        /// Converts an enum member array to a <see cref="string"/> array (using ToString).
        /// </summary>
        protected static ICollection<string> ConvertEnumCollectionToStringCollection<TEnum>(ICollection<TEnum> enumArray)
        {
            string[] stringArr = new string[enumArray.Count];

            for (int i = 0; i < enumArray.Count; i++)
                stringArr[i] = enumArray.ElementAt(i).ToString();

            return stringArr;
        }
    }
}