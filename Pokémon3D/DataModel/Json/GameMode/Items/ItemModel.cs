using System.Runtime.Serialization;
using Pokémon3D.DataModel.Json.GameMode.Items.SpecialItems;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Items
{
    /// <summary>
    /// The data model for an item that can be obtained by the player and stored in the inventory.
    /// </summary>
    [DataContract]
    class ItemModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2)]
        public string PluralName;

        [DataMember(Order = 3)]
        public int Price;

        [DataMember(Order = 4)]
        public TextureSourceModel Texture;

        [DataMember(Order = 5)]
        public string Description;

        /// <summary>
        /// The script that gets executed when the item gets used.
        /// </summary>
        [DataMember(Order = 6)]
        public string ScriptBinding;

        [DataMember(Order = 7)]
        public InventoryDefinitionModel InventoryData;

        [DataMember(Order = 8)]
        public ItemUsageModel Usage;

        [DataMember(Order = 9)]
        public ItemClassificationModel Classification;

        #region Special item data

        // The data items in this region could be null, check before use.

        [DataMember(Order = 10)]
        public BerryModel BerryData;

        [DataMember(Order = 11)]
        public PlateModel PlateData;

        [DataMember(Order = 12)]
        public TechnicalMachineModel TMData;

        [DataMember(Order = 13)]
        public MegaStoneModel MegaStoneData;

        #endregion
    }
}
