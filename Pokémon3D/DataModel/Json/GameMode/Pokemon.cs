using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Pokémon3D.DataModel.Json.GameMode
{
    #region Enums

    /// <summary>
    /// The different experience types of a Pokémon.
    /// </summary>
    public enum ExperienceType
    {
        Fast,
        MediumFast,
        MediumSlow,
        Slow
    }

    /// <summary>
    /// EggGroups a Pokémon can have to define its breeding compatibility.
    /// </summary>
    public enum EggGroup
    {
        Monster,
        Water1,
        Water2,
        Water3,
        Bug,
        Flying,
        Field,
        Fairy,
        Grass,
        Undiscovered,
        HumanLike,
        Mineral,
        Amorphous,
        Ditto,
        Dragon,
        GenderUnknown,
        None
    }

    /// <summary>
    /// The conditions to test an evolution for.
    /// </summary>
    public enum EvolutionConditionType
    {
        Level,
        Friendship,
        Item,
        HoldItem,
        Place,
        Trade,
        Gender,
        AtkDef,
        DefAtk,
        DefEqualsAtk,
        Move,
        DayTime,
        InParty,
        InPartyType
    }

    /// <summary>
    /// The trigger for an evolution condition to activate.
    /// </summary>
    public enum EvolutionTrigger
    {
        None,
        LevelUp,
        Trading,
        ItemUse
    }

    #endregion

    /// <summary>
    /// The data model for a Pokémon definition.
    /// </summary>
    [DataContract]
    class PokemonModel : JsonDataModel
    {
        /// <summary>
        /// The name of the Pokémon.
        /// </summary>
        [DataMember(Order = 0)]
        public string Name { get; private set; }

        /// <summary>
        /// The number of this Pokémon in the national Pokédex.
        /// </summary>
        [DataMember(Order = 1)]
        public int Number { get; private set; }

        /// <summary>
        /// The primary type of this Pokémon.
        /// </summary>
        [DataMember(Order = 2)]
        public string Type1 { get; private set; }

        /// <summary>
        /// The secondary type of this Pokémon.
        /// </summary>
        [DataMember(Order = 3)]
        public string Type2 { get; private set; }

        /// <summary>
        /// The base stats that dictate the strength of this Pokémon.
        /// </summary>
        [DataMember(Order = 4)]
        public PokemonStatSetModel BaseStats { get; private set; }

        #region ExperienceType

        [DataMember(Order = 5, Name = "ExperienceType")]
        private string _experienceType;

        /// <summary>
        /// The experience type of this Pokémon, determining how fast it will grow.
        /// </summary>
        public ExperienceType ExperienceType
        {
            get { return ConvertStringToEnum<ExperienceType>(_experienceType); }
        }

        #endregion

        /// <summary>
        /// The base experience value to calculate receiving Exp when defeating this Pokémon.
        /// </summary>
        [DataMember(Order = 6)]
        public int BaseExperience { get; private set; }

        /// <summary>
        /// The catch rate of this Pokémon.
        /// </summary>
        /// <returns></returns>
        [DataMember(Order = 7)]
        public int CatchRate { get; private set; }

        #region EggGroup

        private List<string> _eggGroups;

        public List<EggGroup> EggGroups
        {
            get { return (List<EggGroup>)ConvertStringCollectionToEnumCollection<EggGroup>(_eggGroups); }
        }

        #endregion

        public int BaseEggSteps { get; set; }

        public int EggPokemon { get; set; }

        public int Devolution { get; set; }

        public bool IsBreedable { get; set; }

        public bool IsGenderless { get; set; }

        public double MaleChance { get; set; }

        public List<int> Abilities { get; set; }

        public List<int> EggMoves { get; set; }

        public List<int> MachineMoves { get; set; }

        public List<int> TutorMoves { get; set; }

        public List<LevelUpMoveModel> LevelUpMoves { get; set; }

        public PokemonStatSetModel RewardEV { get; set; }

        public PokedexEntryModel PokedexEntry { get; set; }

        public bool IsLegendary { get; set; }

        public List<EvolutionConditionModel> EvolutionConditions { get; set; }

        public List<HeldItemModel> HeldItems { get; set; }
    }

    /// <summary>
    /// A data model for an item held by a wild Pokémon.
    /// </summary>
    [DataContract]
    class HeldItemModel : JsonDataModel
    {
        /// <summary>
        /// The Id of the item.
        /// </summary>
        [DataMember(Order = 0)]
        public int ItemId { get; private set; }

        /// <summary>
        /// The chance of this item appearing.
        /// </summary>
        /// <remarks>This is not a percentage, but rather relative to all other objects in the same chance list.</remarks>
        [DataMember(Order = 1)]
        public int Chance { get; private set; }
    }

    /// <summary>
    /// The data model for an evolution condition of a Pokémon.
    /// </summary>
    [DataContract]
    class EvolutionConditionModel : JsonDataModel
    {
        [DataMember(Order = 0, Name = "ConditionType")]
        private string _conditionType;

        /// <summary>
        /// The state to check for this condition.
        /// </summary>
        public EvolutionConditionType ConditionType
        {
            get { return ConvertStringToEnum<EvolutionConditionType>(_conditionType); }
        }

        /// <summary>
        /// The condition that has to be reached with the value returned from the condition type.
        /// </summary>
        [DataMember(Order = 1)]
        public string Condition { get; private set; }

        /// <summary>
        /// The Id of the Pokémon this Pokémon will evolve into.
        /// </summary>
        [DataMember(Order = 2)]
        public int Evolution { get; private set; }

        [DataMember(Order = 3, Name = "Trigger")]
        private string _trigger;

        /// <summary>
        /// The trigger that initiates the check for this condition.
        /// </summary>
        public EvolutionTrigger Trigger
        {
            get { return ConvertStringToEnum<EvolutionTrigger>(_trigger); }
        }
    }

    /// <summary>
    /// The data model for a move a Pokémon learns at level up.
    /// </summary>
    [DataContract]
    class LevelUpMoveModel : JsonDataModel
    {
        /// <summary>
        /// The level the Pokémon learns the move at.
        /// </summary>
        [DataMember(Order = 0)]
        public int Level { get; private set; }

        /// <summary>
        /// The Id of the move.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; private set; }
    }

    /// <summary>
    /// The data model for a stat definition for a Pokémon.
    /// </summary>
    [DataContract]
    class PokemonStatSetModel : JsonDataModel
    {
        /// <summary>
        /// The Health Points stat.
        /// </summary>
        [DataMember(Order = 0)]
        public int HP { get; private set; }

        /// <summary>
        /// The Attack stat.
        /// </summary>
        [DataMember(Order = 1)]
        public int Atk { get; private set; }

        /// <summary>
        /// The Defense stat.
        /// </summary>
        [DataMember(Order = 2)]
        public int Def { get; private set; }

        /// <summary>
        /// The Special Attack stat.
        /// </summary>
        [DataMember(Order = 3)]
        public int SpAtk { get; private set; }

        /// <summary>
        /// The Special Defense stat.
        /// </summary>
        [DataMember(Order = 4)]
        public int SpDef { get; private set; }

        /// <summary>
        /// The Speed stat.
        /// </summary>
        [DataMember(Order = 5)]
        public int Speed { get; private set; }
    }

    /// <summary>
    /// The data model for a Pokédex entry of a Pokémon.
    /// </summary>
    [DataContract]
    class PokedexEntryModel : JsonDataModel
    {
        /// <summary>
        /// The description text of the entry.
        /// </summary>
        /// <returns></returns>
        [DataMember(Order = 0)]
        public string Text { get; private set; }

        /// <summary>
        /// The species of this Pokémon.
        /// </summary>
        /// <returns></returns>
        [DataMember(Order = 1)]
        public string Species { get; private set; }

        /// <summary>
        /// The height of this Pokémon.
        /// </summary>
        [DataMember(Order = 2)]
        public double Height { get; private set; }

        /// <summary>
        /// The weight of this Pokémon.
        /// </summary>
        [DataMember(Order = 3)]
        public double Weight { get; private set; }

        /// <summary>
        /// The color associated with this Pokémon.
        /// </summary>
        [DataMember(Order = 4)]
        public ColorModel Color { get; private set; }

        /// <summary>
        /// The body style of this Pokémon. Defined BodyStyles are used.
        /// </summary>
        [DataMember(Order = 5)]
        public string BodyStyle { get; private set; }
    }
}