using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// Palette applied to the entity at a specific season.
    /// </summary>
    [DataContract]
    class EntitySeasonPaletteModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Season;

        [DataMember(Order = 1)]
        public string TexturePath;
    }
}
