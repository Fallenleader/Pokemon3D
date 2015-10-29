using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Items.SpecialItems
{
    [DataContract]
    class TechnicalMachineModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public int MoveId;

        [DataMember(Order = 1)]
        public bool IsHM;
    }
}
