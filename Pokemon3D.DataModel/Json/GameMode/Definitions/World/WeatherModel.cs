using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The data model for a weather definition.
    /// </summary>
    [DataContract]
    public class WeatherModel : JsonDataModel
    {
        [DataMember(Order = 0, Name = "WeatherType")]
        private string _weatherType;
        
        public WeatherType WeatherType
        {
            get { return ConvertStringToEnum<WeatherType>(_weatherType); }
        }

        [DataMember(Order = 1)]
        public int Chance;
    }
}
