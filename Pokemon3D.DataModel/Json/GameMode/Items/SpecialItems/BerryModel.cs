using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Items.SpecialItems
{
    [DataContract]
    public class BerryModel : JsonDataModel
    {
        /// <summary>
        /// The time it takes this berry item to grow one stage when planted.
        /// </summary>
        [DataMember(Order = 0)]
        public int PhaseTime;

        /// <summary>
        /// The minimum amount of berries to yield from a plant.
        /// </summary>
        [DataMember(Order = 1)]
        public int MinBerries;

        /// <summary>
        /// The maximum amount of berries to yield from a plant.
        /// </summary>
        [DataMember(Order = 2)]
        public int MaxBerries;

        [DataMember(Order = 3, Name = "Flavour")]
        private string _flavour;

        /// <summary>
        /// The flavour of this berry item, that influences if a Pokémon likes it or not.
        /// </summary>
        public BerryFlavour Flavour
        {
            get { return ConvertStringToEnum<BerryFlavour>(_flavour); }
            set { _flavour = value.ToString(); }
        }

        [DataMember(Order = 4)]
        public string Size;

        [DataMember(Order = 5)]
        public string Firmness;

        /// <summary>
        /// Refer to <see cref="Definitions.TypeModel"/>.
        /// </summary>
        [DataMember(Order = 6)]
        public string Type;

        [DataMember(Order = 7)]
        public int Power;
    }
}
