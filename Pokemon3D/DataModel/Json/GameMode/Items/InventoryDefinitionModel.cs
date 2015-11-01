using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Items
{
    [DataContract]
    class InventoryDefinitionModel : JsonDataModel
    {
        [DataMember(Name = "ItemCategory", Order = 0)]
        private string _itemCategory;

        public ItemCategory ItemCategory
        {
            get { return ConvertStringToEnum<ItemCategory>(_itemCategory); }
            set { _itemCategory = value.ToString(); }
        }

        [DataMember(Name = "BattleItemCategory", Order = 1)]
        private string _battleItemCategory;

        public BattleItemCategory BattleItemCategory
        {
            get { return ConvertStringToEnum<BattleItemCategory>(_battleItemCategory); }
            set { _battleItemCategory = value.ToString(); }
        }

        [DataMember(Order = 2)]
        public int SortValue;
    }
}
