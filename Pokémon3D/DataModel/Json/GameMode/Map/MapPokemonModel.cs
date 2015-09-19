using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    [DataContract]
    class MapPokemonModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public bool ShowFollower { get; set; }

        [DataMember(Order = 1)]
        public bool WildInGrass { get; set; }

        [DataMember(Order = 3)]
        public bool WildOnFloor { get; set; }

        [DataMember(Order = 2)]
        public bool WildInWater { get; set; }

        [DataMember(Order = 4)]
        public int WildAbilityChance { get; set; }
    }
}
