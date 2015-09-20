using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    [DataContract]
    class MapPokemonModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public bool ShowFollower ;

        [DataMember(Order = 1)]
        public bool WildInGrass ;

        [DataMember(Order = 3)]
        public bool WildOnFloor ;

        [DataMember(Order = 2)]
        public bool WildInWater ;

        [DataMember(Order = 4)]
        public int WildAbilityChance ;
    }
}
