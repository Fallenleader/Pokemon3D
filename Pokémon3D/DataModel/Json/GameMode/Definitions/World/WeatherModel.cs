using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a weather definition.
    /// </summary>
    [DataContract]
    class WeatherModel : JsonDataModel
    {
        [DataMember(Order = 0, Name = "WeatherType")]
        private string _weatherType;
        
        public WeatherType WeatherType
        {
            get { return ConvertStringToEnum<WeatherType>(_weatherType); }
        }
        
        [DataMember(Order = 1)]
        public int Chance { get; private set; }
    }
}
