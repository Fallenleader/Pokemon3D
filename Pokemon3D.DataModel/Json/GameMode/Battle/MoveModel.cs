using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Battle
{
    /// <summary>
    /// Data model for a move a Pokémon can use in battle.
    /// </summary>
    [DataContract]
    public class MoveModel : JsonDataModel
    {
        /* Members:
         x Id
         x Name
         x Type[]
         x Targets (TargetType)
         x Power
         x Accuracy
         x PP
         x Category (Physical etc)
         - ContestData
         x Description
         x CriticalChance
         x IsHMMove
         x Priority
         x TimesToAttack
         x Tags (can be retrieved by scripts to test for similar attack patterns)
        */

        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2)]
        public string Description;

        [DataMember(Order = 3)]
        public string[] Types;

        [DataMember(Name = "MoveCategory", Order = 4)]
        private string _moveCategory;

        public MoveCategory MoveCategory
        {
            get { return ConvertStringToEnum<MoveCategory>(_moveCategory); }
            set { _moveCategory = value.ToString(); }
        }

        [DataMember(Order = 5)]
        public int Power;

        [DataMember(Order = 6)]
        public int Accuracy;

        [DataMember(Order = 7)]
        public int PP;

        [DataMember(Order = 8)]
        public int Priority;

        [DataMember(Order = 9)]
        public int CriticalChance;

        [DataMember(Order = 10)]
        public int TimesToAttack;
        
        [DataMember(Order = 11)]
        private string _target;

        public TargetType Target
        {
            get { return ConvertStringToEnum<TargetType>(_target); }
            set { _target = value.ToString(); }
        }

        [DataMember(Order = 12)]
        public bool IsHMMove;

        [DataMember(Order = 13)]
        public string ScriptBinding;

        [DataMember(Order = 14)]
        public string[] Tags;
    }
}
