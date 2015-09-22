using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Pokemon
{
    /// <summary>
    /// The conditions to test an evolution for.
    /// </summary>
    enum EvolutionConditionType
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
    enum EvolutionTrigger
    {
        None,
        LevelUp,
        Trading,
        ItemUse
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
        public string Condition;

        /// <summary>
        /// The Id of the Pokémon this Pokémon will evolve into.
        /// </summary>
        [DataMember(Order = 2)]
        public int Evolution;

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
}
