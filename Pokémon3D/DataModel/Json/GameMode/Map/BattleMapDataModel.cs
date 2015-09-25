using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// The battle map data for this map. Can be null at runtime.
    /// </summary>
    [DataContract]
    class BattleMapDataModel : JsonDataModel
    {
        [DataMember]
        public string BattleMapFile;

        [DataMember]
        public Vector3Model CameraPosition;
    }
}
