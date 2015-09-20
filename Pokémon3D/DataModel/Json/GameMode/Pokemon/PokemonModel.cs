using System.Runtime.Serialization;

namespace Pokémon3D.DataModel.Json.GameMode.Pokemon
{

    /// <summary>
    /// The different experience types of a Pokémon.
    /// </summary>
    enum ExperienceType
    {
        Fast,
        MediumFast,
        MediumSlow,
        Slow
    }
    
    /// <summary>
    /// The data model for a Pokémon definition.
    /// </summary>
    [DataContract]
    class PokemonModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Name;
        
        [DataMember(Order = 1)]
        public int Number;

        [DataMember(Order = 2)]
        public PokedexEntryModel PokedexEntry;

        [DataMember(Order = 3)]
        public string InitScript;

        [DataMember(Order = 4)]
        public PokemonFormModel[] Forms;

        [DataMember(Order = 5)]
        public HeldItemModel[] HeldItems;

        [DataMember(Order = 6, Name = "ExperienceType")]
        private string _experienceType;
        
        public ExperienceType ExperienceType
        {
            get { return ConvertStringToEnum<ExperienceType>(_experienceType); }
        }
        
        [DataMember(Order = 7)]
        public EvolutionConditionModel[] EvolutionConditions;

        [DataMember(Order = 8)]
        public int BaseFriendship;

        [DataMember(Order = 9)]
        public PokemonStatSetModel RewardEV;

        [DataMember(Order = 10)]
        public double IsMale;

        [DataMember(Order = 11)]
        public bool IsGenderless;

        [DataMember(Order = 12)]
        public bool CanBreed;

        [DataMember(Order = 13)]
        public int BaseEggSteps;

        [DataMember(Order = 14)]
        public int EggPokemon;

        [DataMember(Order = 15)]
        public int Devolution;

        [DataMember(Order = 16)]
        public string[] EggGroups;
    }
}
