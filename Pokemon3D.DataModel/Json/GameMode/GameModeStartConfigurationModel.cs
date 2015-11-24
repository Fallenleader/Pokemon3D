using System;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode
{
    [DataContract]
    public class GameModeStartConfigurationModel : JsonDataModel<GameModeStartConfigurationModel>
    {
        /// <summary>
        /// The path to the startup map of the GameMode (relative to Maps\, no file extension).
        /// </summary>
        [DataMember(Order = 0)]
        public string Map;

        /// <summary>
        /// The path to the startup script of the GameMode (relative to Scripts\, no file extension).
        /// </summary>
        [DataMember(Order = 1)]
        public string Script;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
