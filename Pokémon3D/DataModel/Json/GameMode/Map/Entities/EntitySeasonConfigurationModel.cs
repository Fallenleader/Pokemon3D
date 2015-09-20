using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map.Entities
{
    /// <summary>
    /// Palette applied to the entity at a specific season.
    /// </summary>
    [DataContract]
    class EntitySeasonPaletteModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Season ;

        [DataMember(Order = 1)]
        public string TexturePath ;
    }
}
