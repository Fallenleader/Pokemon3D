using System;

namespace Pokémon3D.DataModel.Json
{
    /// <summary>
    /// An exception thrown when an error occurs while loading Json data.
    /// </summary>
    class JsonDataLoadException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="JsonDataLoadException"/> class.
        /// </summary>
        /// <param name="inner">The inner exception.</param>
        /// <param name="jsonData">The data that the serializer was trying to load.</param>
        /// <param name="targetType">The target type.</param>
        public JsonDataLoadException(Exception inner, string jsonData, Type targetType)
            : base("An exception occured trying to read Json data into an internal format. Please check that the input data is correct.", inner)
        {
            Data.Add("Target type", targetType.Name);
            Data.Add("Json data", jsonData);
        }
    }
}
